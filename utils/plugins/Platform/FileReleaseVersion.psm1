#requires -Version 7.0
#requires -PSEdition Core

<#
.SYNOPSIS
    Loads release version from a repo-root VERSION file into shared context.

.DESCRIPTION
    Reads a single-line semver from the configured versionFilePath (default
    repo-root VERSION). Useful for repositories without .csproj or package.json
    version metadata. Declares providesVersion = $true so the engine can
    discover it as the single release version source.
#>

if (-not (Get-Command Import-PluginDependency -ErrorAction SilentlyContinue)) {
    $srcDir = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
        $pluginSupportModulePath = Join-Path $srcDir "modules/Engine/PluginSupport.psm1"
    if (Test-Path $pluginSupportModulePath -PathType Leaf) {
        Import-Module $pluginSupportModulePath -Force -Global -ErrorAction Stop
    }
}

function Get-VersionFileSemverInternal {
    param(
        [Parameter(Mandatory = $true)]
        [string]$VersionFilePath
    )

    if (-not (Test-Path $VersionFilePath -PathType Leaf)) {
        throw "FileReleaseVersion: VERSION file not found at '$VersionFilePath'."
    }

    $version = (Get-Content -Path $VersionFilePath -Raw -Encoding UTF8).Trim()
    if ([string]::IsNullOrWhiteSpace($version)) {
        throw "FileReleaseVersion: VERSION file is empty at '$VersionFilePath'."
    }

    $version = $version -replace '^[vV]', ''
    if ($version -notmatch '^\d+\.\d+\.\d+') {
        throw "FileReleaseVersion: version '$version' in '$VersionFilePath' is not a valid semver."
    }

    return $version
}

function Get-PluginMetadata {
    return [pscustomobject]@{ providesVersion = $true }
}

function Invoke-Plugin {
    param(
        [Parameter(Mandatory = $true)]
        $Settings
    )

    Import-PluginDependency -ModuleName "Logging" -RequiredCommand "Write-Log"
    Import-PluginDependency -ModuleName "EngineContext" -RequiredCommand "Set-EngineState"

    $shared = $Settings.context
    $versionFileSetting = if ($Settings.versionFilePath) {
        $Settings.versionFilePath
    }
    else {
        '..\..\..\VERSION'
    }

    $versionFilePaths = @(Resolve-RelativePaths -Value $versionFileSetting -BasePath $shared.scriptDir)
    if ($versionFilePaths.Count -eq 0) {
        throw "FileReleaseVersion plugin requires 'versionFilePath' (repo-root VERSION file) in scriptSettings.json."
    }

    $versionFilePath = $versionFilePaths[0]
    Write-Log -Level "INFO" -Message "Reading version from VERSION file (versionFilePath)..."
    $version = Get-VersionFileSemverInternal -VersionFilePath $versionFilePath

    Set-EngineState -Context $shared -Name 'version' -Value $version
    Set-EngineFact -Context $shared -Namespace 'release' -Name 'versionFilePath' -Value $versionFilePath -Overwrite Replace -LegacyProperty 'versionFilePath'
    Write-Log -Level "OK" -Message "  Release version loaded by FileReleaseVersion plugin: $version"
}

Export-ModuleMember -Function Invoke-Plugin, Get-PluginMetadata
