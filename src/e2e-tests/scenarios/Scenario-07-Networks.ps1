Register-PodmanE2eScenario -Id 'Networks' -Description 'Create/list/inspect/connect/disconnect/remove networks' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $netName = "e2e-net-$suffix"
  $ctrName = "e2e-net-ctr-$suffix"
  $networkName = $null
  $containerId = $null

  try {
    Write-E2eLog -Kind Step 'Ensure alpine + container'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $created = New-PodmanContainer -Name $ctrName -Image $image -Command @('sh', '-c', 'sleep 300')
    $containerId = $created.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($containerId)) 'Container Id missing'

    Use-PodmanE2eCmdlet 'Start-PodmanContainer'
    Start-PodmanContainer -Name $containerId

    Write-E2eLog -Kind Step 'New-PodmanNetwork'
    Use-PodmanE2eCmdlet 'New-PodmanNetwork'
    $net = New-PodmanNetwork -Name $netName
    $networkName = if ($net.Name) { $net.Name } else { $netName }
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($networkName)) 'Network name missing'

    Write-E2eLog -Kind Step 'Get-PodmanNetworkList / Get-PodmanNetwork'
    Use-PodmanE2eCmdlet 'Get-PodmanNetworkList'
    $list = @(Get-PodmanNetworkList)
    Assert-PodmanE2eTrue ($list.Count -gt 0) 'Network list empty'

    Use-PodmanE2eCmdlet 'Get-PodmanNetwork'
    $inspect = Get-PodmanNetwork -Name $networkName
    Assert-PodmanE2eTrue ($null -ne $inspect) 'Network inspect null'

    Write-E2eLog -Kind Step 'Connect-PodmanNetwork / Disconnect-PodmanNetwork'
    Use-PodmanE2eCmdlet 'Connect-PodmanNetwork'
    Connect-PodmanNetwork -Name $networkName -Container $containerId

    Use-PodmanE2eCmdlet 'Disconnect-PodmanNetwork'
    Disconnect-PodmanNetwork -Name $networkName -Container $containerId -Force

    Write-E2eLog -Kind Step 'Remove-PodmanNetwork'
    Use-PodmanE2eCmdlet 'Remove-PodmanNetwork'
    Remove-PodmanNetwork -Name $networkName -Confirm:$false
    $networkName = $null

    Write-E2eLog -Kind Step 'Negative: missing network'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanNetwork'
      Get-PodmanNetwork -Name "no-such-network-$suffix"
    }

    Write-E2eLog -Kind Ok 'Networks scenario passed'
  }
  finally {
    if ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
      }
      catch { }
    }
    if ($networkName) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanNetwork'
        Remove-PodmanNetwork -Name $networkName -Confirm:$false
      }
      catch { }
    }
  }
}
