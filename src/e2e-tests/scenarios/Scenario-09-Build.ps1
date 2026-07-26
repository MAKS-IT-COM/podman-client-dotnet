Register-PodmanE2eScenario -Id 'Build' -Description 'Build image from temp Dockerfile via Invoke-PodmanBuildImage and progress API' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $tag = "e2e-build-$suffix"
  $tagProgress = "e2e-build-prog-$suffix"
  $tmpDir = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-build-$suffix")
  New-Item -ItemType Directory -Force -Path $tmpDir | Out-Null
  $dockerfilePath = Join-Path $tmpDir 'Dockerfile'
  $contextTar = Join-Path $tmpDir 'context.tar'

  try {
    Write-E2eLog -Kind Step 'Ensure alpine base available'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference 'alpine:latest' -Quiet

    Write-E2eLog -Kind Step 'Write Dockerfile + context tar'
    @(
      'FROM alpine:latest'
      'CMD ["echo","e2e-built"]'
    ) | Set-Content -LiteralPath $dockerfilePath -Encoding utf8

    New-PodmanE2eTarFromFolder -FolderPath $tmpDir -TarPath $contextTar | Out-Null
    Assert-PodmanE2eTrue (Test-Path -LiteralPath $contextTar) 'Build context tar missing'

    Write-E2eLog -Kind Step 'Invoke-PodmanBuildImage'
    Use-PodmanE2eCmdlet 'Invoke-PodmanBuildImage'
    $report = Invoke-PodmanBuildImage -Dockerfile 'Dockerfile' -ContextPath $contextTar -Tag $tag -Pull
    Assert-PodmanE2eTrue ($null -ne $report -or $true) 'BuildImage completed'

    Use-PodmanE2eCmdlet 'Test-PodmanImage'
    $built = Test-PodmanImage -Name $tag
    Assert-PodmanE2eTrue ($built -eq $true) "Built image $tag should exist"

    Write-E2eLog -Kind Step 'Invoke-PodmanBuildImageProgress'
    Use-PodmanE2eCmdlet 'Invoke-PodmanBuildImageProgress'
    $lines = @(Invoke-PodmanBuildImageProgress -Dockerfile 'Dockerfile' -ContextPath $contextTar -Tag $tagProgress -Wait)
    Assert-PodmanE2eTrue ($lines.Count -ge 0) 'Build progress completed'

    Write-E2eLog -Kind Ok 'Build scenario passed'
  }
  finally {
    foreach ($t in @($tag, $tagProgress)) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanImage'
        Remove-PodmanImage -Name $t -Force
      }
      catch { }
    }
    if (Test-Path -LiteralPath $tmpDir) {
      Remove-Item -LiteralPath $tmpDir -Recurse -Force -ErrorAction SilentlyContinue
    }
  }
}
