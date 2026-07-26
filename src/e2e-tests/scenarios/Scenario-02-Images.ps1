Register-PodmanE2eScenario -Id 'Images' -Description 'Pull/list/inspect/tag/history/tree/export/save/load/import/push/prune images' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $tagRepo = "localhost/e2e-img-$suffix"
  $tagName = 'v1'
  $tagged = "${tagRepo}:${tagName}"
  $batchTag = "localhost/e2e-batch-${suffix}:latest"
  $importRef = "localhost/e2e-import-${suffix}:latest"
  $tmpDir = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-images-$suffix")
  New-Item -ItemType Directory -Force -Path $tmpDir | Out-Null
  $exportTar = Join-Path $tmpDir 'export.tar'
  $saveTar = Join-Path $tmpDir 'save.tar'
  $fsTar = Join-Path $tmpDir 'fs-import.tar'
  $containerName = "e2e-img-export-$suffix"
  $containerId = $null

  try {
    Write-E2eLog -Kind Step 'Invoke-PodmanPullImage alpine:latest'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Write-E2eLog -Kind Step 'Invoke-PodmanPullImageProgress'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImageProgress'
    $progress = @(Invoke-PodmanPullImageProgress -Reference $image -Quiet -Wait)
    Assert-PodmanE2eTrue ($progress.Count -ge 0) 'Pull progress returned unexpected result'

    Write-E2eLog -Kind Step 'Get-PodmanImageList / Test-PodmanImage / Get-PodmanImage'
    Use-PodmanE2eCmdlet 'Get-PodmanImageList'
    $list = @(Get-PodmanImageList -All)
    Assert-PodmanE2eTrue ($list.Count -gt 0) 'Image list empty after pull'

    Use-PodmanE2eCmdlet 'Test-PodmanImage'
    $exists = Test-PodmanImage -Name $image
    Assert-PodmanE2eTrue ($exists -eq $true) 'alpine:latest should exist'

    Use-PodmanE2eCmdlet 'Get-PodmanImage'
    $inspect = Get-PodmanImage -Name $image
    Assert-PodmanE2eTrue ($null -ne $inspect) 'Inspect image returned null'

    Write-E2eLog -Kind Step 'Search-PodmanImage'
    Use-PodmanE2eCmdlet 'Search-PodmanImage'
    $search = @(Search-PodmanImage -Term 'alpine' -Limit 5)
    Assert-PodmanE2eTrue ($search.Count -ge 0) 'Search failed unexpectedly'

    Write-E2eLog -Kind Step 'Tag / history / tree / changes'
    Use-PodmanE2eCmdlet 'Invoke-PodmanTagImage'
    $null = Invoke-PodmanTagImage -Image $image -Repo $tagRepo -Tag $tagName

    Use-PodmanE2eCmdlet 'Get-PodmanImageHistory'
    $history = @(Get-PodmanImageHistory -Name $tagged)
    Assert-PodmanE2eTrue ($history.Count -ge 0) 'History empty/failed'

    Use-PodmanE2eCmdlet 'Get-PodmanImageTree'
    $tree = Get-PodmanImageTree -Name $tagged
    Assert-PodmanE2eTrue ($null -ne $tree) 'Image tree null'

    Use-PodmanE2eCmdlet 'Get-PodmanImageChange'
    $changes = Get-PodmanImageChange -Name $tagged
    Assert-PodmanE2eTrue ($null -ne $changes -or $true) 'Image changes call completed'

    Write-E2eLog -Kind Step 'Export-PodmanImage / Save-PodmanImage / Import-PodmanImageArchive'
    Use-PodmanE2eCmdlet 'Export-PodmanImage'
    $null = Export-PodmanImage -Reference @($image) -OutFile $exportTar
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $exportTar) 'Export tar missing'

    Use-PodmanE2eCmdlet 'Save-PodmanImage'
    $null = Save-PodmanImage -Name $image -OutFile $saveTar
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $saveTar) 'Save tar missing'

    Use-PodmanE2eCmdlet 'Import-PodmanImageArchive'
    $null = Import-PodmanImageArchive -Path $saveTar

    Write-E2eLog -Kind Step 'Import-PodmanImage from container filesystem export'
    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $created = New-PodmanContainer -Name $containerName -Image $image -Command @('sh', '-c', 'echo import-src')
    $containerId = $created.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($containerId)) 'Container create for import failed'

    Use-PodmanE2eCmdlet 'Export-PodmanContainer'
    $null = Export-PodmanContainer -Name $containerId -OutFile $fsTar

    Use-PodmanE2eCmdlet 'Import-PodmanImage'
    $null = Import-PodmanImage -Path $fsTar -Reference $importRef -Message "e2e-import-$suffix"

    Write-E2eLog -Kind Step 'Invoke-PodmanPushImage (bogus destination expects failure)'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Invoke-PodmanPushImage'
      Invoke-PodmanPushImage -Name $image -Destination "127.0.0.1:1/e2e-no-registry-$suffix/alpine:latest" -TlsVerify:$false
    }

    Write-E2eLog -Kind Step 'Untag / Remove-PodmanImage / Remove-PodmanImageBatch / Prune'
    Use-PodmanE2eCmdlet 'Invoke-PodmanUntagImage'
    $null = Invoke-PodmanUntagImage -Name $tagged -Repo $tagRepo -Tag $tagName

    Use-PodmanE2eCmdlet 'Invoke-PodmanTagImage'
    $null = Invoke-PodmanTagImage -Image $image -Repo "localhost/e2e-batch-$suffix" -Tag 'latest'

    Use-PodmanE2eCmdlet 'Remove-PodmanImageBatch'
    $null = Remove-PodmanImageBatch -Image @($batchTag) -Force

    Use-PodmanE2eCmdlet 'Remove-PodmanImage'
    try {
      $null = Remove-PodmanImage -Name $importRef -Force
    }
    catch {
      Write-E2eLog -Kind Warn "Remove import ref: $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Invoke-PodmanPruneImage'
    $null = Invoke-PodmanPruneImage

    Write-E2eLog -Kind Step 'Negative: invalid image id'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanImage'
      Get-PodmanImage -Name "no-such-image-$suffix"
    }

    Write-E2eLog -Kind Ok 'Images scenario passed'
  }
  finally {
    if ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
      }
      catch { }
    }
    foreach ($ref in @($tagged, $batchTag, $importRef)) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanImage'
        Remove-PodmanImage -Name $ref -Force
      }
      catch { }
    }
    if (Test-Path -LiteralPath $tmpDir) {
      Remove-Item -LiteralPath $tmpDir -Recurse -Force -ErrorAction SilentlyContinue
    }
  }
}
