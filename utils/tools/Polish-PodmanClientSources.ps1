#Requires -Version 7.0
param(
  [string]$ProjectDir = (Join-Path $PSScriptRoot '..\..\src\PodmanClient')
)

$ErrorActionPreference = 'Stop'
$rootNs = 'MaksIT.PodmanClientDotNet'

function Convert-HumanName([string]$name) {
  $s = $name -replace 'Dto$', '' -replace 'Request$', ' request' -replace 'Response$', ' response'
  return ($s -creplace '([A-Z])', ' $1').Trim()
}

function Get-TypeSummary([string]$typeName, [string]$kind) {
  if ($typeName -match 'Request$') { return "Libpod API request body for $(Convert-HumanName $typeName)." }
  if ($typeName -match 'Response$') { return "Libpod API response body for $(Convert-HumanName $typeName)." }
  if ($typeName -match 'Dto$') { return "Deserialized Podman libpod API payload ($(Convert-HumanName $typeName))." }
  if ($kind -eq 'model') { return "Libpod container or image specification model ($(Convert-HumanName $typeName))." }
  return "Podman libpod API type ($(Convert-HumanName $typeName))."
}

function Polish-ModelFile([string]$path) {
  $text = Get-Content -LiteralPath $path -Raw
  if ([string]::IsNullOrWhiteSpace($text)) { return }

  $nsMatch = [regex]::Match($text, 'namespace\s+([\w.]+)\s*\{')
  if (-not $nsMatch.Success) {
    if ($text -match 'namespace\s+([\w.]+)\s*;') {
      $ns = $Matches[1]
      $inner = $text -replace '(?s).*?namespace\s+[\w.]+\s*;\s*', ''
    } else { return }
  } else {
    $ns = $nsMatch.Groups[1].Value
    $start = $nsMatch.Index + $nsMatch.Length
    $inner = $text.Substring($start)
    $inner = $inner.Trim()
    if ($inner.EndsWith('}')) { $inner = $inner.Substring(0, $inner.LastIndexOf('}')).Trim() }
  }

  $inner = $inner -replace '(?m)^using\s+[\w.]+\s*;\s*\r?\n', ''
  $inner = $inner.Trim()

  $out = New-Object System.Collections.Generic.List[string]
  $out.Add("namespace $ns;")
  $out.Add('')

  if ($inner -notmatch '/// <summary>') {
    $typeMatch = [regex]::Match($inner, 'public\s+(?:sealed\s+)?class\s+(\w+)')
    if ($typeMatch.Success) {
      $summary = Get-TypeSummary $typeMatch.Groups[1].Value 'model'
      $out.Add('/// <summary>')
      $out.Add("/// $summary")
      $out.Add('/// </summary>')
      $out.Add('')
    }
  } else {
    foreach ($line in ($inner -split '\r?\n')) { $out.Add($line) }
    Set-Content -LiteralPath $path -Value (($out -join "`n").TrimEnd() + "`n") -NoNewline
    return
  }

  $inner = $inner -replace '(?m)^\s+', '  '
  $inner = $inner -replace 'public\s+(class|sealed class)\s+(\w+)\s*\r?\n\s*\{', 'public $1 $2 {'
  $inner = $inner -replace 'public\s+(class|sealed class)\s+(\w+)\s*$', 'public $1 $2 {'

  if ($inner -notmatch '\}\s*$') { $inner = $inner + "`n}" }

  foreach ($line in ($inner -split '\r?\n')) { $out.Add($line) }

  Set-Content -LiteralPath $path -Value (($out -join "`n").TrimEnd() + "`n") -NoNewline
}

function Polish-DtoFile([string]$path) {
  $text = Get-Content -LiteralPath $path -Raw
  if ($text -match '/// <summary>') { return }

  $matches = [regex]::Matches($text, 'public\s+(?:sealed\s+)?class\s+(\w+)')
  foreach ($m in $matches) {
    $typeName = $m.Groups[1].Value
    $summary = Get-TypeSummary $typeName 'dto'
    $doc = "/// <summary>`n/// $summary`n/// </summary>`n"
    $text = $text -replace (
      '(?m)^(\s*)(public\s+(?:sealed\s+)?class\s+' + [regex]::Escape($typeName) + '\s*\{)'
    ), "$doc`$1`$2"
  }
  Set-Content -LiteralPath $path -Value $text -NoNewline
}

function Remove-RootNamespaceLine([string]$path) {
  $text = Get-Content -LiteralPath $path -Raw
  if ($text -notmatch "(?m)^namespace $([regex]::Escape($rootNs));\s*\r?\n") { return }
  $text = $text -replace "(?m)^namespace $([regex]::Escape($rootNs));\s*\r?\n", ''
  Set-Content -LiteralPath $path -Value $text -NoNewline
}

Get-ChildItem -LiteralPath (Join-Path $ProjectDir 'Models') -Recurse -Filter '*.cs' | ForEach-Object {
  Polish-ModelFile $_.FullName
}

Get-ChildItem -LiteralPath (Join-Path $ProjectDir 'Dtos') -Recurse -Filter '*.cs' | ForEach-Object {
  if ($_.FullName -notmatch '/// <summary>') { Polish-DtoFile $_.FullName }
}

$rootFiles = @(
  'PodmanClient.cs', 'IPodmanClient.cs', 'IPodmanClientConfiguration.cs'
) + (Get-ChildItem -LiteralPath $ProjectDir -Filter 'PodmanClient.*.cs').Name

foreach ($name in $rootFiles) {
  $path = Join-Path $ProjectDir $name
  if (Test-Path -LiteralPath $path) { Remove-RootNamespaceLine $path }
}

Get-ChildItem -LiteralPath (Join-Path $ProjectDir 'Abstractions') -Filter '*.cs' | ForEach-Object {
  Remove-RootNamespaceLine $_.FullName
}

Write-Host 'Polish complete.'
