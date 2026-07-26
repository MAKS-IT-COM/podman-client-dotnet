# Shared helpers for Podman E2E. Dot-sourced by Test-PodmanE2E.ps1 and scenarios.

$script:PodmanE2eScenarioRegistry = [System.Collections.Generic.List[hashtable]]::new()
$script:PodmanE2eCmdletHits = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)

function Clear-PodmanE2eScenarioRegistry {
  $script:PodmanE2eScenarioRegistry.Clear()
  $script:PodmanE2eCmdletHits.Clear()
}

function Register-PodmanE2eScenario {
  param(
    [Parameter(Mandatory)][string] $Id,
    [Parameter(Mandatory)][string] $Description,
    [Parameter(Mandatory)][scriptblock] $ScriptBlock
  )
  $script:PodmanE2eScenarioRegistry.Add(@{
    Id          = $Id
    Description = $Description
    ScriptBlock = $ScriptBlock
  }) | Out-Null
}

function Write-E2eLog {
  param(
    [Parameter(Mandatory)][string] $Message,
    [ValidateSet('Default', 'Step', 'Ok', 'Warn')]
    [string] $Kind = 'Default'
  )
  $ts = (Get-Date).ToUniversalTime().ToString('o')
  $line = "[$ts] $Message"
  switch ($Kind) {
    'Step' { Write-Host $line -ForegroundColor Cyan }
    'Ok' { Write-Host $line -ForegroundColor Green }
    'Warn' { Write-Host $line -ForegroundColor Yellow }
    default { Write-Host $line }
  }
}

function New-PodmanE2eSuffix {
  [guid]::NewGuid().ToString('N').Substring(0, 8)
}

function Use-PodmanE2eCmdlet {
  param([Parameter(Mandatory)][string] $Name)
  [void]$script:PodmanE2eCmdletHits.Add($Name)
}

function Assert-PodmanE2eTrue {
  param([bool] $Condition, [string] $Message)
  if (-not $Condition) { throw $Message }
}

function Assert-PodmanE2eError {
  param(
    [Parameter(Mandatory)][scriptblock] $ScriptBlock,
    [string] $Message = 'Expected cmdlet to fail.'
  )
  $failed = $false
  try {
    & $ScriptBlock 2>&1 | ForEach-Object {
      if ($_ -is [System.Management.Automation.ErrorRecord]) { $failed = $true }
    }
  }
  catch {
    $failed = $true
  }
  if (-not $failed) { throw $Message }
}

function New-PodmanE2eTarFromFolder {
  param([Parameter(Mandatory)][string] $FolderPath, [Parameter(Mandatory)][string] $TarPath)
  if (Test-Path -LiteralPath $TarPath) { Remove-Item -LiteralPath $TarPath -Force }
  & tar -cf $TarPath -C $FolderPath . 
  if ($LASTEXITCODE -ne 0) { throw "tar failed creating $TarPath" }
  return $TarPath
}
