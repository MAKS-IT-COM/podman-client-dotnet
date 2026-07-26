Register-PodmanE2eScenario -Id 'Exec' -Description 'Create/start/inspect/resize exec and Invoke-PodmanExecSession' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $name = "e2e-exec-$suffix"
  $containerId = $null

  try {
    Write-E2eLog -Kind Step 'Ensure alpine + sleep container'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $created = New-PodmanContainer -Name $name -Image $image -Command @('sh', '-c', 'sleep 300')
    $containerId = $created.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($containerId)) 'Container Id missing'

    Use-PodmanE2eCmdlet 'Start-PodmanContainer'
    Start-PodmanContainer -Name $containerId

    Write-E2eLog -Kind Step 'New-PodmanExec + Invoke-PodmanExecSession (echo exec-ok)'
    Use-PodmanE2eCmdlet 'New-PodmanExec'
    $exec = New-PodmanExec -ContainerName $containerId -Cmd @('echo', 'exec-ok')
    $execId = $exec.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($execId)) 'CreateExecResponseDto.Id missing'

    Use-PodmanE2eCmdlet 'Invoke-PodmanExecSession'
    $out = Invoke-PodmanExecSession -ExecId $execId
    Assert-PodmanE2eTrue ("$out" -match 'exec-ok') "Exec session output missing exec-ok: $out"

    Write-E2eLog -Kind Step 'TTY exec: Resize + Start -Detach + Get-PodmanExec'
    Use-PodmanE2eCmdlet 'New-PodmanExec'
    $ttyExec = New-PodmanExec -ContainerName $containerId -Cmd @('sh', '-c', 'echo tty-ok') -Tty
    $ttyId = $ttyExec.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($ttyId)) 'TTY exec Id missing'

    Use-PodmanE2eCmdlet 'Resize-PodmanExec'
    try {
      Resize-PodmanExec -ExecId $ttyId -Height 40 -Width 120
    }
    catch {
      Write-E2eLog -Kind Warn "Resize-PodmanExec: $($_.Exception.Message)"
      Assert-PodmanE2eError {
        Use-PodmanE2eCmdlet 'Resize-PodmanExec'
        Resize-PodmanExec -ExecId "no-such-exec-$suffix" -Height 24 -Width 80
      }
    }

    Use-PodmanE2eCmdlet 'Start-PodmanExec'
    Start-PodmanExec -ExecId $ttyId -Detach -Tty

    Use-PodmanE2eCmdlet 'Get-PodmanExec'
    $inspected = Get-PodmanExec -ExecId $ttyId
    Assert-PodmanE2eTrue ($null -ne $inspected) 'Inspect exec null'

    Write-E2eLog -Kind Step 'Negative: invalid exec / container'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'New-PodmanExec'
      New-PodmanExec -ContainerName "no-such-container-$suffix" -Cmd @('echo', 'x')
    }
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanExec'
      Get-PodmanExec -ExecId "no-such-exec-$suffix"
    }

    Write-E2eLog -Kind Ok 'Exec scenario passed'
  }
  finally {
    if ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
      }
      catch { }
    }
  }
}
