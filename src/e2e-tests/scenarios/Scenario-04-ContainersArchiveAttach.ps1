Register-PodmanE2eScenario -Id 'ContainersArchiveAttach' -Description 'Container archive put/get/extract/export and attach/session streaming' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $sleepName = "e2e-arch-$suffix"
  $echoName = "e2e-attach-$suffix"
  $sleepId = $null
  $echoId = $null
  $tmpDir = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-arch-$suffix")
  New-Item -ItemType Directory -Force -Path $tmpDir | Out-Null
  $payloadDir = Join-Path $tmpDir 'payload'
  New-Item -ItemType Directory -Force -Path $payloadDir | Out-Null
  Set-Content -LiteralPath (Join-Path $payloadDir 'hello.txt') -Value "e2e-archive-$suffix" -NoNewline
  $tarPath = Join-Path $tmpDir 'payload.tar'
  $getArchive = Join-Path $tmpDir 'from-container.tar'
  $exportCtr = Join-Path $tmpDir 'container-export.tar'
  $attachOut = Join-Path $tmpDir 'attach.out'

  try {
    Write-E2eLog -Kind Step 'Ensure alpine image'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    New-PodmanE2eTarFromFolder -FolderPath $payloadDir -TarPath $tarPath | Out-Null

    Write-E2eLog -Kind Step 'Create sleep container for archive ops'
    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $sleepCreated = New-PodmanContainer -Name $sleepName -Image $image -Command @('sh', '-c', 'sleep 300')
    $sleepId = $sleepCreated.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($sleepId)) 'Sleep container Id missing'

    Use-PodmanE2eCmdlet 'Start-PodmanContainer'
    Start-PodmanContainer -Name $sleepId

    Write-E2eLog -Kind Step 'Invoke-PodmanExtractArchive / Set-PodmanContainerArchive / Put / Get'
    Use-PodmanE2eCmdlet 'Invoke-PodmanExtractArchive'
    Invoke-PodmanExtractArchive -ContainerId $sleepId -Path '/e2e' -FilePath $tarPath -Pause:$false

    Use-PodmanE2eCmdlet 'Set-PodmanContainerArchive'
    Set-PodmanContainerArchive -ContainerId $sleepId -Path '/e2e-set' -FilePath $tarPath -Pause:$false

    Use-PodmanE2eCmdlet 'Invoke-PodmanPutContainerArchive'
    Invoke-PodmanPutContainerArchive -ContainerId $sleepId -Path '/e2e-put' -FilePath $tarPath -Pause:$false

    Use-PodmanE2eCmdlet 'Get-PodmanContainerArchive'
    $null = Get-PodmanContainerArchive -Name $sleepId -Path '/e2e' -OutFile $getArchive
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $getArchive) 'Get-PodmanContainerArchive OutFile missing'

    Use-PodmanE2eCmdlet 'Export-PodmanContainer'
    $null = Export-PodmanContainer -Name $sleepId -OutFile $exportCtr
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $exportCtr) 'Export-PodmanContainer OutFile missing'

    Write-E2eLog -Kind Step 'Invoke-PodmanContainerAttach (logs after exit)'
    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $echoCreated = New-PodmanContainer -Name $echoName -Image $image -Command @('sh', '-c', 'echo hello-attach')
    $echoId = $echoCreated.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($echoId)) 'Echo container Id missing'

    Use-PodmanE2eCmdlet 'Start-PodmanContainer'
    Start-PodmanContainer -Name $echoId
    Start-Sleep -Seconds 2

    Use-PodmanE2eCmdlet 'Wait-PodmanContainer'
    try {
      $null = Wait-PodmanContainer -Name $echoId -Condition 'exited'
    }
    catch {
      Write-E2eLog -Kind Warn "Wait echo container: $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Invoke-PodmanContainerAttach'
    $null = Invoke-PodmanContainerAttach -Name $echoId -Logs -Stream:$false -OutFile $attachOut
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $attachOut) 'Attach OutFile missing'
    $attachText = Get-Content -LiteralPath $attachOut -Raw
    Assert-PodmanE2eTrue ("$attachText" -match 'hello-attach') "Attach logs missing hello-attach: $attachText"

    Write-E2eLog -Kind Step 'Negative attach/session on invalid container'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Invoke-PodmanContainerSession'
      Invoke-PodmanContainerSession -Name "no-such-container-$suffix" -Stdin:$false
    }

    Write-E2eLog -Kind Ok 'ContainersArchiveAttach scenario passed'
  }
  finally {
    foreach ($id in @($sleepId, $echoId)) {
      if ($id) {
        try {
          Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
          Remove-PodmanContainer -Name $id -Force -Ignore
        }
        catch { }
      }
    }
    if (Test-Path -LiteralPath $tmpDir) {
      Remove-Item -LiteralPath $tmpDir -Recurse -Force -ErrorAction SilentlyContinue
    }
  }
}
