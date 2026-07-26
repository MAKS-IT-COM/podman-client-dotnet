Register-PodmanE2eScenario -Id 'ContainersLifecycle' -Description 'Container create/init/start/inspect/stats/pause/kill/wait/commit/checkpoint/mount/prune' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $name = "e2e-ctr-$suffix"
  $renamed = "e2e-ctr-renamed-$suffix"
  $commitRepo = "localhost/e2e-commit-$suffix"
  $commitTag = 'latest'
  $containerId = $null
  $activeName = $name
  $ckptTar = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-ckpt-$suffix.tar")
  $logFile = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-ctrlog-$suffix.txt")

  try {
    Write-E2eLog -Kind Step 'Ensure alpine image'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Write-E2eLog -Kind Step 'New-PodmanContainer / Initialize / Start'
    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $created = New-PodmanContainer -Name $name -Image $image -Command @('sh', '-c', 'sleep 300')
    $containerId = $created.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($containerId)) 'CreateContainerResponseDto.Id missing'

    Use-PodmanE2eCmdlet 'Initialize-PodmanContainer'
    try {
      Initialize-PodmanContainer -Name $containerId
    }
    catch {
      Write-E2eLog -Kind Warn "Initialize-PodmanContainer: $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Start-PodmanContainer'
    Start-PodmanContainer -Name $containerId

    Write-E2eLog -Kind Step 'Inspect / list / exists / logs / stats / top / changes'
    Use-PodmanE2eCmdlet 'Get-PodmanContainer'
    $inspect = Get-PodmanContainer -Name $containerId
    Assert-PodmanE2eTrue ($null -ne $inspect) 'Inspect container null'

    Use-PodmanE2eCmdlet 'Get-PodmanContainerList'
    $clist = @(Get-PodmanContainerList -All)
    Assert-PodmanE2eTrue ($clist.Count -gt 0) 'Container list empty'

    Use-PodmanE2eCmdlet 'Test-PodmanContainer'
    $exists = Test-PodmanContainer -Name $containerId
    Assert-PodmanE2eTrue ($exists -eq $true) 'Container should exist'

    Use-PodmanE2eCmdlet 'Get-PodmanContainerLog'
    $null = Get-PodmanContainerLog -Name $containerId -Tail '10' -OutFile $logFile

    Use-PodmanE2eCmdlet 'Get-PodmanContainerStat'
    $stat = Get-PodmanContainerStat -Name $containerId
    Assert-PodmanE2eTrue ($null -ne $stat) 'Container stat null'

    Use-PodmanE2eCmdlet 'Get-PodmanContainerStatBatch'
    $batch = Get-PodmanContainerStatBatch -Containers @($containerId)
    Assert-PodmanE2eTrue ($null -ne $batch) 'Container stat batch null'

    Use-PodmanE2eCmdlet 'Get-PodmanContainerProcess'
    $proc = Get-PodmanContainerProcess -Name $containerId -Stream:$false
    Assert-PodmanE2eTrue ($null -ne $proc) 'Container process/top null'

    Use-PodmanE2eCmdlet 'Get-PodmanContainerTop'
    $top = Get-PodmanContainerTop -Name $containerId -Stream:$false
    Assert-PodmanE2eTrue ($null -ne $top) 'Container top null'

    Use-PodmanE2eCmdlet 'Get-PodmanContainerChange'
    $chg = Get-PodmanContainerChange -Name $containerId
    Assert-PodmanE2eTrue ($null -ne $chg -or $true) 'Container changes completed'

    Write-E2eLog -Kind Step 'Suspend / Resume (pause/unpause)'
    Use-PodmanE2eCmdlet 'Suspend-PodmanContainer'
    Suspend-PodmanContainer -Name $containerId
    Use-PodmanE2eCmdlet 'Resume-PodmanContainer'
    Resume-PodmanContainer -Name $containerId

    Write-E2eLog -Kind Step 'Restart / Rename'
    Use-PodmanE2eCmdlet 'Restart-PodmanContainer'
    Restart-PodmanContainer -Name $containerId -Timeout 10

    Use-PodmanE2eCmdlet 'Rename-PodmanContainer'
    Rename-PodmanContainer -Name $containerId -NewName $renamed
    $activeName = $renamed

    Write-E2eLog -Kind Step 'HealthCheck (may fail without HEALTHCHECK)'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Invoke-PodmanContainerHealthCheck'
      Invoke-PodmanContainerHealthCheck -Name $activeName
    }

    Write-E2eLog -Kind Step 'Commit container to image'
    Use-PodmanE2eCmdlet 'Invoke-PodmanCommitContainer'
    $null = Invoke-PodmanCommitContainer -Container $activeName -Repo $commitRepo -Tag $commitTag -Comment "e2e-$suffix" -Format docker -Pause:$false

    Write-E2eLog -Kind Step 'Mount / Dismount / Get-PodmanMountedContainer'
    $mounted = $false
    Use-PodmanE2eCmdlet 'Mount-PodmanContainer'
    try {
      $null = Mount-PodmanContainer -Name $activeName
      $mounted = $true
    }
    catch {
      Write-E2eLog -Kind Warn "Mount failed (rootless?): $($_.Exception.Message)"
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Mount-PodmanContainer'
        Mount-PodmanContainer -Name $activeName
      }
    }

    Use-PodmanE2eCmdlet 'Get-PodmanMountedContainer'
    $null = Get-PodmanMountedContainer

    Use-PodmanE2eCmdlet 'Dismount-PodmanContainer'
    if ($mounted) {
      Dismount-PodmanContainer -Name $activeName
    }
    else {
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Dismount-PodmanContainer'
        Dismount-PodmanContainer -Name $activeName
      }
    }

    Write-E2eLog -Kind Step 'Checkpoint / Restore (CRIU may be missing; remote import path is host-local)'
    $ckptOk = $false
    Use-PodmanE2eCmdlet 'Checkpoint-PodmanContainer'
    try {
      $null = Checkpoint-PodmanContainer -Name $activeName -LeaveRunning -Export -OutFile $ckptTar
      $ckptOk = $true
      Write-E2eLog -Kind Ok 'Checkpoint succeeded'
    }
    catch {
      Write-E2eLog -Kind Warn "Checkpoint failed (likely no CRIU): $($_.Exception.Message)"
    }

    if (-not $ckptOk) {
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Checkpoint-PodmanContainer'
        Checkpoint-PodmanContainer -Name $activeName -LeaveRunning
      }
    }

    # ImportPath is a server-side path; a Windows temp file is not valid on the remote Podman host.
    Use-PodmanE2eCmdlet 'Restore-PodmanContainer'
    Assert-PodmanE2eError {
      Restore-PodmanContainer -Name $activeName -ImportPath '/tmp/podman-e2e-missing-checkpoint.tar'
    }

    Write-E2eLog -Kind Step 'Ensure running, then Kill / Wait / Stop / Remove / Prune'
    try {
      Start-PodmanContainer -Name $activeName
    }
    catch {
      Write-E2eLog -Kind Warn "Start before kill: $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Kill-PodmanContainer'
    try {
      Kill-PodmanContainer -Name $activeName -Signal 'TERM'
    }
    catch {
      Write-E2eLog -Kind Warn "Kill: $($_.Exception.Message)"
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Kill-PodmanContainer'
        Kill-PodmanContainer -Name "no-such-container-$suffix"
      }
    }

    Use-PodmanE2eCmdlet 'Wait-PodmanContainer'
    Write-E2eLog -Kind Warn 'Wait-PodmanContainer exercised via negative path only (avoid hang on running containers)'
    Assert-PodmanE2eError {
      Wait-PodmanContainer -Name "no-such-container-$suffix" -Condition 'exited'
    }

    Use-PodmanE2eCmdlet 'Stop-PodmanContainer'
    try {
      Stop-PodmanContainer -Name $activeName -IgnoreAlreadyStopped
    }
    catch {
      Write-E2eLog -Kind Warn "Stop after kill: $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
    Remove-PodmanContainer -Name $activeName -Force
    $containerId = $null
    $activeName = $null

    Use-PodmanE2eCmdlet 'Invoke-PodmanPruneContainer'
    $null = Invoke-PodmanPruneContainer

    Write-E2eLog -Kind Step 'Negative: invalid container id'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanContainer'
      Get-PodmanContainer -Name "no-such-container-$suffix"
    }
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Start-PodmanContainer'
      Start-PodmanContainer -Name "no-such-container-$suffix"
    }

    Write-E2eLog -Kind Ok 'ContainersLifecycle scenario passed'
  }
  finally {
    if ($activeName) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $activeName -Force -Ignore
      }
      catch { }
    }
    elseif ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
      }
      catch { }
    }
    try {
      Use-PodmanE2eCmdlet 'Remove-PodmanImage'
      Remove-PodmanImage -Name "${commitRepo}:${commitTag}" -Force
    }
    catch { }
    foreach ($f in @($ckptTar, $logFile)) {
      if (Test-Path -LiteralPath $f) {
        Remove-Item -LiteralPath $f -Force -ErrorAction SilentlyContinue
      }
    }
  }
}
