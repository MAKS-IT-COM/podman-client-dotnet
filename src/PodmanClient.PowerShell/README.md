# MaksIT.PodmanClientDotNet.PowerShell

Binary PowerShell module wrapping **PodmanClient.DotNet** (`IPodmanClient`).

## Requirements

- PowerShell 7 hosted on **.NET 10**
- Reachable Podman REST API (`podman system service`) — E2E baseline **Podman 5.4.0** (`Connect-Podman` default `ApiVersion` = `v5.4.0`)

## Quick start

```powershell
dotnet build src/PodmanClient.PowerShell/PodmanClient.PowerShell.csproj
Import-Module .\src\PodmanClient.PowerShell\bin\Debug\net10.0\MaksIT.PodmanClientDotNet.PowerShell.psd1 -Force
Connect-Podman -BaseAddress 'http://192.168.2.128:8080' -ApiVersion 'v5.4.0'
Test-PodmanConnection
Get-PodmanVersion
Disconnect-Podman
```

Cmdlets mirror the .NET client domains (system, images, containers, exec, volumes, networks, pods, build, manifests, generate). See `CmdletsToExport` in the `.psd1` and live E2E under `src/e2e-tests/`.
