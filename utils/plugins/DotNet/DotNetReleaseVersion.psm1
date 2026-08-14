#requires -Version 7.0
#requires -PSEdition Core

<#
.SYNOPSIS
    Loads release version from an SDK-style .csproj into shared context.

.DESCRIPTION
    Dedicated version-loading plugin. Reads <Version> from the first configured
    projectFiles entry and writes it (plus the resolved projectFiles) to the
    shared runtime context. Declares providesVersion = $true so the engine can
    discover it as the single release version source.
#>

if (-not (Get-Command Import-PluginDependency -ErrorAction SilentlyContinue)) {
    $srcDir = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
        $pluginSupportModulePath = Join-Path $srcDir "modules/Engine/PluginSupport.psm1"
    if (Test-Path $pluginSupportModulePath -PathType Leaf) {
        Import-Module $pluginSupportModulePath -Force -Global -ErrorAction Stop
    }
}

function Get-CsprojPropertyValueInternal {
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

function Get-CsprojVersionInternal {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ProjectPath
    )

    if (-not (Test-Path $ProjectPath -PathType Leaf)) {
        throw "DotNetReleaseVersion: project file not found at '$ProjectPath'."
    }

    if ([System.IO.Path]::GetExtension($ProjectPath) -ne ".csproj") {
        throw "DotNetReleaseVersion: configured project file is not a .csproj file: '$ProjectPath'."
    }

    [xml]$csproj = Get-Content $ProjectPath
    $version = Get-CsprojPropertyValueInternal -Csproj $csproj -PropertyName "Version"

    if ([string]::IsNullOrWhiteSpace([string]$version)) {
        throw "DotNetReleaseVersion: <Version> not found in '$ProjectPath'."
    }

    return [string]$version
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
    $projectFiles = @(Resolve-RelativePaths -Value $Settings.projectFiles -BasePath $shared.scriptDir)
    if ($projectFiles.Count -eq 0) {
        throw "DotNetReleaseVersion plugin requires 'projectFiles' (first .csproj with <Version>) in scriptSettings.json."
    }

    Write-Log -Level "INFO" -Message "Reading version from SDK-style project file (projectFiles)..."
    $version = Get-CsprojVersionInternal -ProjectPath $projectFiles[0]

    Set-EngineState -Context $shared -Name 'version' -Value $version
    Set-EngineFact -Context $shared -Namespace 'dotnet' -Name 'projectFiles' -Value $projectFiles -Overwrite Replace -LegacyProperty 'projectFiles'
    Write-Log -Level "OK" -Message "  Release version loaded by DotNetReleaseVersion plugin: $version"
}

Export-ModuleMember -Function Invoke-Plugin, Get-PluginMetadata
