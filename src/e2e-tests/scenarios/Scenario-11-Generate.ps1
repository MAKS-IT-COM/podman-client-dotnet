Register-PodmanE2eScenario -Id 'Generate' -Description 'Generate systemd/kube for a container; PlayKube minimal yaml then cleanup' -ScriptBlock {
  $suffix = New-PodmanE2eSuffix
  $image = 'alpine:latest'
  $ctrName = "e2e-gen-ctr-$suffix"
  $podName = "e2e-playkube-$suffix"
  $containerId = $null
  $playPod = $null
  $playContainers = @()
  $tmpDir = Join-Path ([System.IO.Path]::GetTempPath()) ("podman-e2e-gen-$suffix")
  New-Item -ItemType Directory -Force -Path $tmpDir | Out-Null
  $kubeYaml = Join-Path $tmpDir 'play.yaml'

  try {
    Write-E2eLog -Kind Step 'Ensure alpine + container for generate'
    Use-PodmanE2eCmdlet 'Invoke-PodmanPullImage'
    $null = Invoke-PodmanPullImage -Reference $image -Quiet

    Use-PodmanE2eCmdlet 'New-PodmanContainer'
    $created = New-PodmanContainer -Name $ctrName -Image $image -Command @('sh', '-c', 'sleep 300')
    $containerId = $created.Id
    Assert-PodmanE2eTrue (-not [string]::IsNullOrWhiteSpace($containerId)) 'Container Id missing'

    Use-PodmanE2eCmdlet 'Start-PodmanContainer'
    Start-PodmanContainer -Name $containerId

    Write-E2eLog -Kind Step 'Invoke-PodmanGenerateSystemd'
    Use-PodmanE2eCmdlet 'Invoke-PodmanGenerateSystemd'
    $systemd = Invoke-PodmanGenerateSystemd -Name $containerId -UseName
    Assert-PodmanE2eTrue ($null -ne $systemd) 'GenerateSystemd returned null'

    Write-E2eLog -Kind Step 'Invoke-PodmanGenerateKube'
    Use-PodmanE2eCmdlet 'Invoke-PodmanGenerateKube'
    $kube = Invoke-PodmanGenerateKube -Name @($containerId)
    Assert-PodmanE2eTrue ($null -ne $kube) 'GenerateKube returned null'

    Write-E2eLog -Kind Step 'Invoke-PodmanPlayKube (minimal pod yaml)'
    @"
apiVersion: v1
kind: Pod
metadata:
  name: $podName
spec:
  restartPolicy: Never
  containers:
  - name: alpine
    image: alpine:latest
    command: ["sleep", "300"]
"@ | Set-Content -LiteralPath $kubeYaml -Encoding utf8

    Use-PodmanE2eCmdlet 'Invoke-PodmanPlayKube'
    $play = Invoke-PodmanPlayKube -Path $kubeYaml -Start
    if ($play) {
      $playPod = if ($play.Pod) { $play.Pod } else { $podName }
      if ($play.Containers) { $playContainers = @($play.Containers) }
    }
    else {
      $playPod = $podName
    }

    Write-E2eLog -Kind Ok 'Generate scenario passed'
  }
  finally {
    foreach ($c in $playContainers) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $c -Force -Ignore
      }
      catch { }
    }
    if ($playPod) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanPod'
        Remove-PodmanPod -Name $playPod -Force -Confirm:$false
      }
      catch {
        try {
          Use-PodmanE2eCmdlet 'Remove-PodmanPod'
          Remove-PodmanPod -Name $podName -Force -Confirm:$false
        }
        catch { }
      }
    }
    if ($containerId) {
      try {
        Use-PodmanE2eCmdlet 'Remove-PodmanContainer'
        Remove-PodmanContainer -Name $containerId -Force -Ignore
      }
      catch { }
    }
    if (Test-Path -LiteralPath $tmpDir) {
      Remove-Item -LiteralPath $tmpDir -Recurse -Force -ErrorAction SilentlyContinue
    }
  }
}
