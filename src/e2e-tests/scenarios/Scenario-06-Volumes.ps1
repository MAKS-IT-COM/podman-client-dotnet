Register-PodmanE2eScenario -Id 'Volumes' -Description 'Create/list/inspect/remove/prune volumes' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $volName = "e2e-vol-$suffix"
  $createdName = $null

  try {
    Write-E2eLog -Kind Step 'New-PodmanVolume'
    Use-PodmanE2eCmdlet 'New-PodmanVolume'
    $vol = New-PodmanVolume -Name $volName
    $createdName = if ($vol.Name) { $vol.Name } else { $volName }
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($createdName)) 'Volume name missing'

    Write-E2eLog -Kind Step 'Get-PodmanVolumeList / Get-PodmanVolume'
    Use-PodmanE2eCmdlet 'Get-PodmanVolumeList'
    $list = @(Get-PodmanVolumeList)
    Assert-PodmanE2eTrue ($list.Count -gt 0) 'Volume list empty'

    Use-PodmanE2eCmdlet 'Get-PodmanVolume'
    $inspect = Get-PodmanVolume -Name $createdName
    Assert-PodmanE2eTrue ($null -ne $inspect) 'Volume inspect null'

    Write-E2eLog -Kind Step 'Remove-PodmanVolume'
    Use-PodmanE2eCmdlet 'Remove-PodmanVolume'
    Remove-PodmanVolume -Name $createdName -Confirm:$false
    $createdName = $null

    Write-E2eLog -Kind Step 'Invoke-PodmanPruneVolume'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPruneVolume'
    $null = Invoke-PodmanPruneVolume

    Write-E2eLog -Kind Step 'Negative: missing volume'
    Assert-PodmanE2eError {
      Use-PodmanE2eCmdlet 'Get-PodmanVolume'
      Get-PodmanVolume -Name "no-such-volume-$suffix"
    }

    Write-E2eLog -Kind Ok 'Volumes scenario passed'
  }
  finally {
    if ($createdName) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanVolume'
        Remove-PodmanVolume -Name $createdName -Force -Confirm:$false
      }
      catch { }
    }
  }
}
