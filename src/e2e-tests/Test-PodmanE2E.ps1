#Requires -Version 7
<#
.SYNOPSIS
  End-to-end tests against a running Podman API via MaksIT.PodmanClientDotNet.PowerShell.

.DESCRIPTION
  Builds the module, connects with PODMAN_TEST_URL, then runs scenarios from scenarios\Scenario-*.ps1.

  Filter:
    pwsh -File .\src\e2e-tests\Test-PodmanE2E.ps1 -Scenario 'System'
    pwsh -File .\src\e2e-tests\Test-PodmanE2E.ps1 -Scenario '*Image*','*Container*'

.EXAMPLE
  $env:PODMAN_TEST_URL = 'http://192.168.2.128:8080'
  pwsh -File .\src\e2e-tests\Test-PodmanE2E.ps1
#>
param(
  [string[]] $Scenario = @('*')
)

# Allow comma-separated values from cmd.exe / single-arg callers: -Scenario System,Images
$Scenario = @(
  $Scenario |
    ForEach-Object { $_ -split ',' } |
    ForEach-Object { $_.Trim().Trim("'").Trim('"') } |
    Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
)
if ($Scenario.Count -eq 0) {
  $Scenario = @('*')
}

$e2eRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
. (Join-Path $e2eRoot 'Podman.E2E.Common.ps1')

if ($PSVersionTable.PSVersion.Major -lt 7) {
  throw 'This script is pwsh-only.'
}

$runtimeFx = [System.Runtime.InteropServices.RuntimeInformation]::FrameworkDescription
if ($runtimeFx -notmatch '\.NET 10\.') {
  throw (
    "The Podman module is net10.0; this pwsh is hosted on: $runtimeFx" + [Environment]::NewLine +
    'Install PowerShell 7 with .NET 10 from https://github.com/PowerShell/PowerShell/releases'
  )
}

$envName = 'PODMAN_TEST_URL'
$url = [Environment]::GetEnvironmentVariable($envName, 'Process')
if ([string]::IsNullOrWhiteSpace($url)) {
  $url = [Environment]::GetEnvironmentVariable($envName, 'User')
}
if ([string]::IsNullOrWhiteSpace($url)) {
  $url = [Environment]::GetEnvironmentVariable($envName, 'Machine')
}

if ([string]::IsNullOrWhiteSpace($url)) {
  throw @"
PODMAN_TEST_URL is not set (Process, User, or Machine).

Example:
  `$env:PODMAN_TEST_URL = 'http://192.168.2.128:8080'
  # must be an absolute http(s) URL (include the scheme)
"@
}

$url = $url.Trim().TrimEnd('/')
if ($url -notmatch '^https?://') {
  throw "PODMAN_TEST_URL must be an absolute http(s) URL. Got: $url"
}

$repoRoot = Resolve-Path (Join-Path $e2eRoot '..\..')
$moduleTfm = 'net10.0'
$relModuleProject = 'src\PodmanClient.PowerShell\PodmanClient.PowerShell.csproj'
$relModuleManifest = "src\PodmanClient.PowerShell\bin\Debug\$moduleTfm\MaksIT.PodmanClientDotNet.PowerShell.psd1"

Write-E2eLog -Kind Step -Message "Build: $relModuleProject"
Push-Location $repoRoot
try {
  $buildOutput = dotnet build $relModuleProject 2>&1
  if ($LASTEXITCODE -ne 0) {
    $buildOutput | ForEach-Object { Write-Host $_ }
    throw "Build failed: $relModuleProject"
  }
  if (-not (Test-Path -LiteralPath $relModuleManifest)) {
    throw "Module manifest not found: $relModuleManifest"
  }
  $moduleManifest = (Resolve-Path -LiteralPath $relModuleManifest).Path
}
finally {
  Pop-Location
}

Write-E2eLog -Kind Ok -Message "Importing module: $moduleManifest"
Import-Module $moduleManifest -Force

$exported = @(Get-Command -Module MaksIT.PodmanClientDotNet.PowerShell | Select-Object -ExpandProperty Name)

Clear-PodmanE2eScenarioRegistry
$scenarioDir = Join-Path $e2eRoot 'scenarios'
if (-not (Test-Path -LiteralPath $scenarioDir)) {
  throw "Scenarios directory missing: $scenarioDir"
}
Get-ChildItem -LiteralPath $scenarioDir -Filter 'Scenario-*.ps1' | Sort-Object Name | ForEach-Object {
  Write-E2eLog -Message "Load scenarios: $($_.Name)"
  . $_.FullName
}

Write-E2eLog -Kind Step -Message "Connect-Podman (base URL: $url, ApiVersion: v5.4.0)"
Connect-Podman -BaseAddress $url -ApiVersion 'v5.4.0'
Use-PodmanE2eCmdlet Connect-Podman
Write-E2eLog -Kind Ok -Message 'Connect-Podman: session ready'

try {
  $ver = Get-PodmanVersion
  $engine = $ver.Version
  if (-not $engine) { $engine = $ver.Components | Where-Object { $_.Name -eq 'Podman Engine' } | Select-Object -ExpandProperty Version -First 1 }
  Write-E2eLog -Message "Server Podman version: $engine (E2E baseline: 5.4.0)"
  if ($engine -and ("$engine" -notlike '5.4*')) {
    Write-E2eLog -Kind Warn "Server is $engine; this suite is validated against Podman 5.4.0"
  }
}
catch {
  Write-E2eLog -Kind Warn "Could not read Get-PodmanVersion: $($_.Exception.Message)"
}

$ErrorActionPreference = 'Stop'
$ran = 0
try {
  foreach ($entry in $script:PodmanE2eScenarioRegistry) {
    $include = $false
    foreach ($p in $Scenario) {
      if ($entry.Id -like $p) {
        $include = $true
        break
      }
    }
    if (-not $include) {
      Write-E2eLog -Kind Warn "Skip (filter): $($entry.Id)"
      continue
    }

    Write-E2eLog -Kind Step -Message "========== Scenario: $($entry.Id) =========="
    Write-E2eLog -Message $entry.Description
    try {
      & $entry.ScriptBlock
      $ran++
    }
    catch {
      Write-E2eLog -Kind Warn "Scenario '$($entry.Id)' FAILED: $($_.Exception.Message)"
      throw
    }
  }

  if ($ran -eq 0) {
    $registered = ($script:PodmanE2eScenarioRegistry | ForEach-Object { $_.Id }) -join ', '
    throw "No scenarios matched -Scenario patterns: $($Scenario -join ', '). Registered: $registered"
  }

  Use-PodmanE2eCmdlet Disconnect-Podman

  $missing = @($exported | Where-Object { -not $script:PodmanE2eCmdletHits.Contains($_) })
  if ($missing.Count -gt 0 -and ($Scenario.Count -eq 1 -and $Scenario[0] -eq '*')) {
    Write-E2eLog -Kind Warn -Message ("Cmdlets not marked Use-PodmanE2eCmdlet in scenarios: " + ($missing -join ', '))
    throw "E2E completeness gate failed: $($missing.Count) cmdlet(s) not exercised."
  }

  Write-E2eLog -Kind Ok -Message "All selected scenarios passed ($ran run)."
}
finally {
  Disconnect-Podman
  Write-E2eLog -Message 'Disconnect-Podman'
}
