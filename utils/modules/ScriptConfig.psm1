#requires -Version 7.0
#requires -PSEdition Core

function Get-ScriptSettings {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$ScriptDir,

        [ValidateSet('single', 'ha')]
        [string]$Mode,

        [Parameter(Mandatory = $false)]
        [string]$SettingsFileName = 'scriptSettings.json'
    )

    $settingsPath = if ($PSBoundParameters.ContainsKey('Mode')) {
        $modePath = Join-Path $ScriptDir "scriptSettings.$Mode.json"
        if (Test-Path -LiteralPath $modePath -PathType Leaf) {
            $modePath
        }
        else {
            Join-Path $ScriptDir $SettingsFileName
        }
    }
    else {
        Join-Path $ScriptDir $SettingsFileName
    }

    if (-not (Test-Path -LiteralPath $settingsPath -PathType Leaf)) {
        throw "Settings file not found: $settingsPath"
    }

    return Get-Content -LiteralPath $settingsPath -Raw | ConvertFrom-Json
}

function Get-HelmSelfDeployPluginFromSettings {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [psobject]$Settings
    )

    if (-not ($Settings.PSObject.Properties.Name -contains 'plugins') -or -not $Settings.plugins) {
        return $null
    }

    foreach ($plugin in @($Settings.plugins)) {
        if ($plugin.name -eq 'HelmSelfDeploy') {
            return $plugin
        }
    }

    return $null
}

function Get-DeployModeFromSettings {
    <#
    .SYNOPSIS
        Resolves cluster deploy profile from HelmSelfDeploy plugin settings or an explicit CLI override.

    .DESCRIPTION
        CI/CD pipelines omit -Mode and read deployMode on the HelmSelfDeploy plugin.
        Manual installs use Invoke-ReleasePackage-Single.bat / Invoke-ReleasePackage-HA.bat.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [psobject]$Settings,

        [ValidateSet('single', 'ha')]
        [string]$ModeOverride
    )

    if ($PSBoundParameters.ContainsKey('ModeOverride')) {
        return $ModeOverride
    }

    $helmSelfDeploy = Get-HelmSelfDeployPluginFromSettings -Settings $Settings
    if ($null -ne $helmSelfDeploy -and $helmSelfDeploy.PSObject.Properties.Name -contains 'deployMode') {
        $mode = [string]$helmSelfDeploy.deployMode
        if (-not [string]::IsNullOrWhiteSpace($mode)) {
            $mode = $mode.Trim().ToLowerInvariant()
            if ($mode -in @('single', 'ha')) {
                return $mode
            }

            throw "HelmSelfDeploy.deployMode must be 'single' or 'ha' (got '$mode')."
        }
    }

    return 'ha'
}

function Resolve-DeployValuesFilePath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$DeploySettingsDir,

        [Parameter(Mandatory = $true)]
        [string]$ValuesFile,

        [Parameter(Mandatory = $true)]
        [ValidateSet('single', 'ha')]
        [string]$DeployMode
    )

    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($ValuesFile)
    $extension = [System.IO.Path]::GetExtension($ValuesFile)
    if ([string]::IsNullOrEmpty($extension)) {
        $extension = '.yaml'
    }

    $valuesPath = Join-Path $DeploySettingsDir "$baseName.$DeployMode$extension"
    if (Test-Path -LiteralPath $valuesPath -PathType Leaf) {
        return $valuesPath
    }

    throw "Deploy values file not found: '$valuesPath'. HelmSelfDeploy expects values.single.yaml or values.ha.yaml beside scriptSettings.json."
}

function Assert-Command {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Command
    )

    if (-not (Get-Command $Command -ErrorAction SilentlyContinue)) {
        throw "Required command '$Command' is missing. Aborting."
    }
}

Export-ModuleMember -Function Get-ScriptSettings, Get-DeployModeFromSettings, Resolve-DeployValuesFilePath, Assert-Command
