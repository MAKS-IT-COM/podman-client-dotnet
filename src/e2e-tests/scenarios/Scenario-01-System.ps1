Register-PodmanE2eScenario -Id 'System' -Description 'System ping, version, info, df, events sample, prune system' -ScriptBlock {
  $tmp = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-events-{0}.json" -f (New-PodmanE2eSuffix))
  try {
    Write-E2eLog -Kind Step 'Test-PodmanConnection'
    Use-PodmanE2eCmdlet 'Test-PodmanConnection'
    $ping = Test-PodmanConnection
    Assert-PodmanE2eTrue ($null -ne $ping -and $ping.Ping) 'Ping did not return OK'

    Write-E2eLog -Kind Step 'Get-PodmanVersion'
    Use-PodmanE2eCmdlet 'Get-PodmanVersion'
    $ver = Get-PodmanVersion
    Assert-PodmanE2eTrue ($null -ne $ver -and -not [string]::IsNullOrWhiteSpace($ver.Version)) 'Version string missing'

    Write-E2eLog -Kind Step 'Get-PodmanInfo'
    Use-PodmanE2eCmdlet 'Get-PodmanInfo'
    $info = Get-PodmanInfo
    Assert-PodmanE2eTrue ($null -ne $info) 'Info returned null'

    Write-E2eLog -Kind Step 'Get-PodmanSystemDiskUsage'
    Use-PodmanE2eCmdlet 'Get-PodmanSystemDiskUsage'
    $df = Get-PodmanSystemDiskUsage
    Assert-PodmanE2eTrue ($null -ne $df) 'System disk usage returned null'

    Write-E2eLog -Kind Step 'Get-PodmanEvent (timed sample)'
    Use-PodmanE2eCmdlet 'Get-PodmanEvent'
    $null = Get-PodmanEvent -OutFile $tmp -ReadTimeoutSeconds 2
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $tmp) 'Events OutFile was not created'

    Write-E2eLog -Kind Step 'Invoke-PodmanPruneSystem'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPruneSystem'
    $null = Invoke-PodmanPruneSystem

    Write-E2eLog -Kind Ok 'System scenario passed'
  }
  finally {
    if (Test-Path -LiteralPath $tmp) {
      Remove-Item -LiteralPath $tmp -Force -ErrorAction SilentlyContinue
    }
  }
}
