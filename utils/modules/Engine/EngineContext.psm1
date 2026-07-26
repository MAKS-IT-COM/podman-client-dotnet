#requires -Version 7.0
#requires -PSEdition Core

<#
.SYNOPSIS
    Helpers to resolve engine semver and relative paths from plugin configuration.

.DESCRIPTION
    Used by New-EngineContext and version plugins:
    - DotNetReleaseVersion plugin -> projectFiles (.csproj <Version>)
    - NpmReleaseVersion plugin -> packageJsonPath (package.json version)
    - FileReleaseVersion plugin -> versionFilePath (repo-root VERSION file)
#>

if (-not (Get-Command Write-Log -ErrorAction SilentlyContinue)) {
    $loggingModulePath = Join-Path (Split-Path $PSScriptRoot -Parent) "Logging.psm1"
    if (Test-Path $loggingModulePath -PathType Leaf) {
        Import-Module $loggingModulePath -Force
    }
}

function Resolve-RelativePaths {
    param(
        [Parameter(Mandatory = $true)]
        [object]$Value,

        [Parameter(Mandatory = $true)]
        [string]$BasePath
    )

    if ($null -eq $Value) {
        return @()
    }

    $rawPaths = @()
    if ($Value -is [System.Collections.IEnumerable] -and -not ($Value -is [string])) {
        $rawPaths += $Value
    }
    else {
        $rawPaths += $Value
    }

    $resolved = @()
    foreach ($p in $rawPaths) {
        if ([string]::IsNullOrWhiteSpace([string]$p)) {
            continue
        }

        $resolved += [System.IO.Path]::GetFullPath((Join-Path $BasePath ([string]$p)))
    }

    return @($resolved)
}

function Get-CsprojPropertyValue {
    param(
        [Parameter(Mandatory = $true)]
        [xml]$Csproj,

        [Parameter(Mandatory = $true)]
        [string]$PropertyName
    )

    # SDK-style .csproj files can have multiple PropertyGroup nodes.
    # Use the first group that defines the requested property.
    $propNode = $Csproj.Project.PropertyGroup |
        Where-Object { $_.$PropertyName } |
        Select-Object -First 1

    if ($propNode) {
        return $propNode.$PropertyName
    }

    return $null
}

function Get-CsprojVersions {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$ProjectFiles
    )

    Write-Log -Level "INFO" -Message "Reading version(s) from SDK-style project files (projectFiles)..."
    $projectVersions = @{}

    foreach ($projectPath in $ProjectFiles) {
        if (-not (Test-Path $projectPath -PathType Leaf)) {
            Write-Error "Project file not found at: $projectPath"
            exit 1
        }

        if ([System.IO.Path]::GetExtension($projectPath) -ne ".csproj") {
            Write-Error "Configured project file is not a .csproj file: $projectPath"
            exit 1
        }

        [xml]$csproj = Get-Content $projectPath
        $version = Get-CsprojPropertyValue -Csproj $csproj -PropertyName "Version"

        if (-not $version) {
            Write-Error "Version not found in $projectPath"
            exit 1
        }

        $projectVersions[$projectPath] = $version
        Write-Log -Level "OK" -Message "  $([System.IO.Path]::GetFileName($projectPath)): $version"
    }

    return $projectVersions
}

function Resolve-DotNetReleaseVersion {
    param(
        [Parameter(Mandatory = $true)]
        [object[]]$Plugins,

        [Parameter(Mandatory = $true)]
        [string]$ScriptDir
    )

    $releaseVersionPlugin = @($Plugins | Where-Object { $_.name -eq 'DotNetReleaseVersion' } | Select-Object -First 1)
    if ($releaseVersionPlugin.Count -eq 0 -or $null -eq $releaseVersionPlugin[0]) {
        Write-Error "Configure a DotNetReleaseVersion plugin in scriptSettings.json with projectFiles."
        exit 1
    }

    $releaseVersionSettings = $releaseVersionPlugin[0]
    $projectFiles = @(Resolve-RelativePaths -Value $releaseVersionSettings.projectFiles -BasePath $ScriptDir)

    if ($projectFiles.Count -eq 0) {
        Write-Error "Configure release version via DotNetReleaseVersion.projectFiles (first .csproj with <Version>)."
        exit 1
    }

    $projectVersions = Get-CsprojVersions -ProjectFiles $projectFiles
    $version = $projectVersions[$projectFiles[0]]

    return [pscustomobject]@{
        version = $version
        source = 'DotNetReleaseVersion'
    }
}

function Resolve-NpmReleaseVersion {
    param(
        [Parameter(Mandatory = $true)]
        [object[]]$Plugins,

        [Parameter(Mandatory = $true)]
        [string]$ScriptDir
    )

    $releaseVersionPlugin = @($Plugins | Where-Object { $_.name -eq 'NpmReleaseVersion' } | Select-Object -First 1)
    if ($releaseVersionPlugin.Count -eq 0 -or $null -eq $releaseVersionPlugin[0]) {
        Write-Error "Configure an NpmReleaseVersion plugin in scriptSettings.json with packageJsonPath."
        exit 1
    }

    $releaseVersionSettings = $releaseVersionPlugin[0]
    $packageJsonPaths = @(Resolve-RelativePaths -Value $releaseVersionSettings.packageJsonPath -BasePath $ScriptDir)

    if ($packageJsonPaths.Count -eq 0) {
        Write-Error "Configure release version via NpmReleaseVersion.packageJsonPath."
        exit 1
    }

    $packageJsonPath = $packageJsonPaths[0]
    if (-not (Test-Path $packageJsonPath -PathType Leaf)) {
        Write-Error "NpmReleaseVersion: package.json not found at: $packageJsonPath"
        exit 1
    }

    Write-Log -Level "INFO" -Message "Reading version from npm package.json (packageJsonPath)..."
    $json = Get-Content -Path $packageJsonPath -Raw -Encoding UTF8 | ConvertFrom-Json
    $version = [string]$json.version
    if ([string]::IsNullOrWhiteSpace($version)) {
        Write-Error "NpmReleaseVersion: 'version' is missing in '$packageJsonPath'."
        exit 1
    }

    if ($version -notmatch '^\d+\.\d+\.\d+') {
        Write-Error "NpmReleaseVersion: version '$version' in '$packageJsonPath' is not a valid semver."
        exit 1
    }

    Write-Log -Level "OK" -Message "  $([System.IO.Path]::GetFileName($packageJsonPath)): $version"

    return [pscustomobject]@{
        version = $version
        source = 'NpmReleaseVersion'
    }
}

function Get-VersionFileSemver {
    param(
        [Parameter(Mandatory = $true)]
        [string]$VersionFilePath
    )

    if (-not (Test-Path $VersionFilePath -PathType Leaf)) {
        Write-Error "FileReleaseVersion: VERSION file not found at: $VersionFilePath"
        exit 1
    }

    $version = (Get-Content -Path $VersionFilePath -Raw -Encoding UTF8).Trim()
    if ([string]::IsNullOrWhiteSpace($version)) {
        Write-Error "FileReleaseVersion: VERSION file is empty at: $VersionFilePath"
        exit 1
    }

    $version = $version -replace '^[vV]', ''
    if ($version -notmatch '^\d+\.\d+\.\d+') {
        Write-Error "FileReleaseVersion: version '$version' in '$VersionFilePath' is not a valid semver."
        exit 1
    }

    return $version
}

function Resolve-FileReleaseVersion {
    param(
        [Parameter(Mandatory = $true)]
        [object[]]$Plugins,

        [Parameter(Mandatory = $true)]
        [string]$ScriptDir
    )

    $releaseVersionPlugin = @($Plugins | Where-Object { $_.name -eq 'FileReleaseVersion' } | Select-Object -First 1)
    if ($releaseVersionPlugin.Count -eq 0 -or $null -eq $releaseVersionPlugin[0]) {
        Write-Error "Configure a FileReleaseVersion plugin in scriptSettings.json with versionFilePath."
        exit 1
    }

    $releaseVersionSettings = $releaseVersionPlugin[0]
    $versionFileSetting = if ($releaseVersionSettings.versionFilePath) {
        $releaseVersionSettings.versionFilePath
    }
    else {
        '..\\..\\..\\VERSION'
    }

    $versionFilePaths = @(Resolve-RelativePaths -Value $versionFileSetting -BasePath $ScriptDir)
    if ($versionFilePaths.Count -eq 0) {
        Write-Error "Configure release version via FileReleaseVersion.versionFilePath (repo-root VERSION file)."
        exit 1
    }

    $versionFilePath = $versionFilePaths[0]
    Write-Log -Level "INFO" -Message "Reading version from VERSION file (versionFilePath)..."
    $version = Get-VersionFileSemver -VersionFilePath $versionFilePath
    Write-Log -Level "OK" -Message "  $([System.IO.Path]::GetFileName($versionFilePath)): $version"

    return [pscustomobject]@{
        version = $version
        source = 'FileReleaseVersion'
    }
}

function Resolve-ReleaseVersion {
    param(
        [Parameter(Mandatory = $true)]
        [object[]]$Plugins,

        [Parameter(Mandatory = $true)]
        [string]$ScriptDir
    )

    $dotnetPlugin = @($Plugins | Where-Object { $_.name -eq 'DotNetReleaseVersion' -and $_.enabled -ne $false })
    $npmPlugin = @($Plugins | Where-Object { $_.name -eq 'NpmReleaseVersion' -and $_.enabled -ne $false })
    $filePlugin = @($Plugins | Where-Object { $_.name -eq 'FileReleaseVersion' -and $_.enabled -ne $false })

    $enabledVersionPlugins = @()
    if ($dotnetPlugin.Count -gt 0) { $enabledVersionPlugins += 'DotNetReleaseVersion' }
    if ($npmPlugin.Count -gt 0) { $enabledVersionPlugins += 'NpmReleaseVersion' }
    if ($filePlugin.Count -gt 0) { $enabledVersionPlugins += 'FileReleaseVersion' }

    if ($enabledVersionPlugins.Count -gt 1) {
        Write-Error "Configure only one release version plugin: $($enabledVersionPlugins -join ', ')."
        exit 1
    }

    if ($dotnetPlugin.Count -gt 0) {
        return Resolve-DotNetReleaseVersion -Plugins $Plugins -ScriptDir $ScriptDir
    }

    if ($npmPlugin.Count -gt 0) {
        return Resolve-NpmReleaseVersion -Plugins $Plugins -ScriptDir $ScriptDir
    }

    if ($filePlugin.Count -gt 0) {
        return Resolve-FileReleaseVersion -Plugins $Plugins -ScriptDir $ScriptDir
    }

    Write-Error "Configure a DotNetReleaseVersion (projectFiles), NpmReleaseVersion (packageJsonPath), or FileReleaseVersion (versionFilePath) plugin in scriptSettings.json."
    exit 1
}

Export-ModuleMember -Function Get-CsprojPropertyValue, Get-CsprojVersions, Get-VersionFileSemver, Resolve-RelativePaths, Resolve-DotNetReleaseVersion, Resolve-NpmReleaseVersion, Resolve-FileReleaseVersion, Resolve-ReleaseVersion



