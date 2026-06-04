#Requires -Version 7.0
param(
  [string]$ModelsDir = (Join-Path $PSScriptRoot '..\..\src\PodmanClient\Models')
)

$ValueTypes = [System.Collections.Generic.HashSet[string]]::new(
  [string[]]@('bool', 'byte', 'sbyte', 'char', 'decimal', 'double', 'float', 'int', 'uint', 'long', 'ulong', 'short', 'ushort')
)

function Add-NullableToProperty([string]$line) {
  if ($line -notmatch '^\s*public\s+(.+?)\s+(\w+)\s*\{\s*get;\s*set;\s*\}\s*$') { return $line }
  $type = $Matches[1].Trim()
  $name = $Matches[2]
  if ($type.EndsWith('?')) { return $line }
  if ($type -match '^(bool|byte|sbyte|char|decimal|double|float|int|uint|long|ulong|short|ushort)(\?)?$') { return $line }

  $nullableType = if ($type.EndsWith('[]')) { $type + '?' } else { $type + '?' }
  $indent = ($line -replace '^(\s*).*', '$1')
  return "${indent}public $nullableType $name { get; set; }"
}

Get-ChildItem -LiteralPath $ModelsDir -Recurse -Filter '*.cs' | ForEach-Object {
  $lines = Get-Content -LiteralPath $_.FullName
  $out = New-Object System.Collections.Generic.List[string]
  foreach ($line in $lines) {
    if ($line -eq '#nullable disable') { continue }
    $out.Add((Add-NullableToProperty $line))
  }
  $text = ($out -join "`n").TrimEnd() + "`n"
  $text = $text -replace '(?m)^  \}\s*$', '}'
  Set-Content -LiteralPath $_.FullName -Value $text -NoNewline
}

Write-Host 'Models nullable annotations applied.'
