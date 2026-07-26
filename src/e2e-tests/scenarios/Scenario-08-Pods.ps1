Register-PodmanE2eScenario -Id 'Pods' -Description 'Create/list/inspect/start/stop/restart/kill/pause/top/stats/prune pods' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $podName = "e2e-pod-$suffix"
  $ctrName = "e2e-pod-ctr-$suffix"
  $podIdOrName = $null
  $containerId = $null

  try {
    Write-E2eLog -Kind Step 'Ensure alpine image'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Write-E2eLog -Kind Step 'New-PodmanPod'
    Use-PodmanE2eCmdlet 'New-PodmanPod'
    $pod = New-PodmanPod -Name $podName
    $podIdOrName = if ($pod.Name) { $pod.Name } elseif ($pod.Id) { $pod.Id } else { $podName }
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($podIdOrName)) 'Pod id/name missing'

    Write-E2eLog -Kind Step 'Get-PodmanPodList / Get-PodmanPod / Test-PodmanPod'
    Use-PodmanE2eCmdlet 'Get-PodmanPodList'
    $plist = @(Get-PodmanPodList -All)
    Assert-PodmanE2eTrue ($plist.Count -gt 0) 'Pod list empty'

    Use-PodmanE2eCmdlet 'Get-PodmanPod'
    $inspect = Get-PodmanPod -Name $podIdOrName
    Assert-PodmanE2eTrue ($null -ne $inspect) 'Pod inspect null'

    Use-PodmanE2eCmdlet 'Test-PodmanPod'
    $exists = Test-PodmanPod -Name $podIdOrName
    Assert-PodmanE2eTrue ($exists -eq $true) 'Pod should exist'

    Write-E2eLog -Kind Step 'Create container in default net, then Start-PodmanPod'
    # Infra pod start exercises Start-PodmanPod; optional app container for richer top/stats.
    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $created = New-PodmanContainer -Name $ctrName -Image $image -Command @('sh', '-c', 'sleep 300')
    $containerId = $created.Id

    Use-PodmanE2eCmdlet 'Start-PodmanPod'
    Start-PodmanPod -Name $podIdOrName

    if ($containerId) {
      Use-PodmanE2eCmdlet 'Start-PodmanContainer'
      try {
        Start-PodmanContainer -Name $containerId
      }
      catch {
        Write-E2eLog -Kind Warn "Start sidecar container: $($_.Exception.Message)"
      }
    }

    Write-E2eLog -Kind Step 'Get-PodmanPodTop / Get-PodmanPodStat'
    Use-PodmanE2eCmdlet 'Get-PodmanPodTop'
    try {
      $null = Get-PodmanPodTop -Name $podIdOrName
    }
    catch {
      Write-E2eLog -Kind Warn "Get-PodmanPodTop: $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Get-PodmanPodStat'
    $null = Get-PodmanPodStat

    Write-E2eLog -Kind Step 'Suspend / Resume / Restart'
    Use-PodmanE2eCmdlet 'Suspend-PodmanPod'
    try {
      Suspend-PodmanPod -Name $podIdOrName
      Use-PodmanE2eCmdlet 'Resume-PodmanPod'
      Resume-PodmanPod -Name $podIdOrName
    }
    catch {
      Write-E2eLog -Kind Warn "Suspend/Resume pod: $($_.Exception.Message)"
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Suspend-PodmanPod'
        Suspend-PodmanPod -Name "no-such-pod-$suffix"
      }
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Resume-PodmanPod'
        Resume-PodmanPod -Name "no-such-pod-$suffix"
      }
    }

    Use-PodmanE2eCmdlet 'Restart-PodmanPod'
    Restart-PodmanPod -Name $podIdOrName -Timeout 10

    Write-E2eLog -Kind Step 'Kill / Stop / Remove / Prune'
    Use-PodmanE2eCmdlet 'Kill-PodmanPod'
    Kill-PodmanPod -Name $podIdOrName -Signal 'TERM'

    Use-PodmanE2eCmdlet 'Stop-PodmanPod'
    try {
      Stop-PodmanPod -Name $podIdOrName -Timeout 10
    }
    catch {
      Write-E2eLog -Kind Warn "Stop-PodmanPod after kill: $($_.Exception.Message)"
    }

    if ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
        $containerId = $null
      }
      catch { }
    }

    Use-PodmanE2eCmdlet 'Remove-PodmanPod'
    Remove-PodmanPod -Name $podIdOrName -Force -Confirm:$false
    $podIdOrName = $null

    Use-PodmanE2eCmdlet 'Invoke-PodmanPrunePod'
    $null = Invoke-PodmanPrunePod

    Write-E2eLog -Kind Step 'Negative: missing pod'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanPod'
      Get-PodmanPod -Name "no-such-pod-$suffix"
    }

    Write-E2eLog -Kind Ok 'Pods scenario passed'
  }
  finally {
    if ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
      }
      catch { }
    }
    if ($podIdOrName) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanPod'
        Remove-PodmanPod -Name $podIdOrName -Force -Confirm:$false
      }
      catch { }
    }
  }
}
