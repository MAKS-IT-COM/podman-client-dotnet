Register-PodmanE2eScenario -Id 'Manifests' -Description 'Create/add/inspect/push/publish/remove manifests' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $manifestName = "e2e-manifest-${suffix}:latest"
  $created = $false

  try {
    Write-E2eLog -Kind Step 'Ensure alpine image'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Write-E2eLog -Kind Step 'New-PodmanManifest'
    Use-PodmanE2eCmdlet 'New-PodmanManifest'
    $null = New-PodmanManifest -Name $manifestName -Image $image
    $created = $true

    Write-E2eLog -Kind Step 'Add-PodmanManifest / Get-PodmanManifest'
    Use-PodmanE2eCmdlet 'Add-PodmanManifest'
    try {
      Add-PodmanManifest -Name $manifestName -Image $image
    }
    catch {
      Write-E2eLog -Kind Warn "Add-PodmanManifest (may already include image): $($_.Exception.Message)"
    }

    Use-PodmanE2eCmdlet 'Get-PodmanManifest'
    $inspect = Get-PodmanManifest -Name $manifestName
    Assert-PodmanE2eTrue ($null -ne $inspect) 'Manifest inspect null'

    Write-E2eLog -Kind Step 'Publish / Push manifest to bogus registry (expect fail)'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Publish-PodmanManifest'
      Publish-PodmanManifest -Name $manifestName -Destination "127.0.0.1:1/e2e-no-registry-$suffix/manifest:latest"
    }

    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Invoke-PodmanPushManifest'
      Invoke-PodmanPushManifest -Name $manifestName -Destination "127.0.0.1:1/e2e-no-registry-$suffix/manifest:push"
    }

    Write-E2eLog -Kind Step 'Remove-PodmanManifest'
    Use-PodmanE2eCmdlet 'Remove-PodmanManifest'
    Remove-PodmanManifest -Name $manifestName -Confirm:$false
    $created = $false

    Write-E2eLog -Kind Step 'Negative: missing manifest'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanManifest'
      Get-PodmanManifest -Name "no-such-manifest-${suffix}:latest"
    }

    Write-E2eLog -Kind Ok 'Manifests scenario passed'
  }
  finally {
    if ($created) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanManifest'
        Remove-PodmanManifest -Name $manifestName -Confirm:$false
      }
      catch { }
    }
  }
}
