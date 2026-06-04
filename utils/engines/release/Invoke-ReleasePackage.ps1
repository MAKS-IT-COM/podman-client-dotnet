#requires -Version 7.0
#requires -PSEdition Core

<#
.SYNOPSIS
    Plugin-driven release engine entry script.
#>

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcDir = (Resolve-Path (Join-Path $scriptDir '..\..')).Path

. (Join-Path $srcDir 'modules/Engine/Import-EngineModules.ps1')
Import-EngineModules -Engine Release

$settings = Get-ScriptSettings -ScriptDir $scriptDir
$configuredPlugins = Get-ConfiguredPlugins -Settings $settings

Write-Log -Level 'STEP' -Message '=================================================='
Write-Log -Level 'STEP' -Message 'RELEASE ENGINE'
Write-Log -Level 'STEP' -Message '=================================================='

$plugins = $configuredPlugins
$engineContext = New-EngineContext -Plugins $plugins -ScriptDir $scriptDir -SrcDir $srcDir -Settings $settings
Write-Log -Level 'OK' -Message 'All pre-flight checks passed!'
$sharedPluginSettings = $engineContext

$releaseStageInitialized = $false
$releaseHadPluginFailures = $false

if ($plugins.Count -eq 0) {
    Write-Log -Level 'WARN' -Message 'No plugins configured in scriptSettings.json.'
}
else {
    for ($pluginIndex = 0; $pluginIndex -lt $plugins.Count; $pluginIndex++) {
        $plugin = $plugins[$pluginIndex]

        if ((Test-IsPublishPlugin -Plugin $plugin) -and -not $releaseStageInitialized) {
            if (Test-PluginRunnable -Plugin $plugin -SharedSettings $sharedPluginSettings -EngineDirectory $scriptDir -WriteLogs:$false) {
                $remainingPlugins = @($plugins[$pluginIndex..($plugins.Count - 1)])
                Initialize-ReleaseStageContext -RemainingPlugins $remainingPlugins -SharedSettings $sharedPluginSettings -ArtifactsDirectory $engineContext.artifactsDirectory -Version $engineContext.version
                $releaseStageInitialized = $true
            }
        }

        $pluginSucceeded = Invoke-ConfiguredPlugin -Plugin $plugin -SharedSettings $sharedPluginSettings -EngineDirectory $scriptDir -ContinueOnError:$false
        if (-not $pluginSucceeded) {
            $releaseHadPluginFailures = $true
            break
        }
    }
}

if (-not $releaseStageInitialized) {
    $noReleasePluginsLogLevel = if ($engineContext.isNonReleaseBranch) { 'INFO' } else { 'WARN' }
    Write-Log -Level $noReleasePluginsLogLevel -Message 'No release-stage initialization ran (no enabled publish plugins reached, or none runnable).'
}

Write-Log -Level 'OK' -Message '=================================================='
if ($releaseHadPluginFailures) {
    Write-Log -Level 'ERROR' -Message 'RELEASE FAILED'
}
elseif ($engineContext.PSObject.Properties.Name -contains 'skipPublishPlugins' -and $engineContext.skipPublishPlugins) {
    Write-Log -Level 'OK' -Message 'RUN COMPLETE (publish skipped by ReleasePublishGuard)'
}
elseif ($engineContext.isNonReleaseBranch) {
    Write-Log -Level 'OK' -Message 'NON-RELEASE RUN COMPLETE'
}
else {
    Write-Log -Level 'OK' -Message 'RELEASE COMPLETE'
}
Write-Log -Level 'OK' -Message '=================================================='

if ($engineContext.isNonReleaseBranch -and -not ($engineContext.PSObject.Properties.Name -contains 'skipPublishPlugins' -and $engineContext.skipPublishPlugins)) {
    $preferredReleaseBranch = Get-PreferredReleaseBranch -EngineContext $engineContext
    Write-Log -Level 'INFO' -Message "For publish, use an allowed branch (see ReleasePublishGuard.branches), e.g. '$preferredReleaseBranch', and satisfy the guard requirements."
}

if ($releaseHadPluginFailures) {
    exit 1
}
