#requires -Version 7.0
#requires -PSEdition Core

<#
.SYNOPSIS
    Loads release version from a repo-root VERSION file into shared context.

.DESCRIPTION
    Reads a single-line semver from the configured versionFilePath (default repo-root VERSION).
    Used by container-only repos without .csproj or package.json.
#>

if (-not (Get-Command Import-PluginDependency -ErrorAction SilentlyContinue)) {
    $srcDir = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
        $pluginSupportModulePath = Join-Path $srcDir "modules/Engine/PluginSupport.psm1"
    if (Test-Path $pluginSupportModulePath -PathType Leaf) {
        Import-Module $pluginSupportModulePath -Force -Global -ErrorAction Stop
    }
}

function Invoke-Plugin {
    param(
        [Parameter(Mandatory = $true)]
        $Settings
    )

    Import-PluginDependency -ModuleName "Logging" -RequiredCommand "Write-Log"
    Import-PluginDependency -ModuleName "EngineContext" -RequiredCommand "Resolve-FileReleaseVersion"

    $shared = $Settings.context
    $resolved = Resolve-FileReleaseVersion -Plugins @($Settings) -ScriptDir $shared.scriptDir
    $versionFilePaths = @(Resolve-RelativePaths -Value $Settings.versionFilePath -BasePath $shared.scriptDir)

    $shared | Add-Member -NotePropertyName version -NotePropertyValue $resolved.version -Force
    if ($versionFilePaths.Count -gt 0) {
        $shared | Add-Member -NotePropertyName versionFilePath -NotePropertyValue $versionFilePaths[0] -Force
    }
    Write-Log -Level "OK" -Message "  Release version loaded by FileReleaseVersion plugin: $($shared.version)"
}

Export-ModuleMember -Function Invoke-Plugin
