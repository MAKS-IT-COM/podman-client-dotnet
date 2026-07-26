# Contributing to PodmanClient.DotNet

Thank you for contributing. This repo follows MaksIT conventions (see `AGENTS.md` and homelab `common/csharp` / `common/maksit-repo-maintenance` skills).

## Development setup

### Prerequisites

- .NET 10 SDK
- Git
- PowerShell 7 hosted on **.NET 10** (for the PowerShell module and E2E)
- Optional: reachable Podman REST API for live E2E (`PODMAN_TEST_URL`)

### Build

```bash
dotnet build src/PodmanClientDotNet.slnx
```

### Unit tests

**RepoUtils test engine** (coverage + badges):

```bash
utils/Invoke-TestEngine.bat
```

**Direct:**

```bash
dotnet test src/PodmanClientDotNet.Tests/PodmanClientDotNet.Tests.csproj
```

Coverage badges in `README.md` are rewritten by the test engine (`CoverageBadges` with `badgeFormat: shields` and `readmePath` in `utils/engines/test/scriptSettings.json`) using `img.shields.io` URLs. Commit the updated README when coverage changes.

### Integration / E2E tests (Podman API VM)

Live API coverage is **PowerShell E2E** under `src/e2e-tests/` (not part of `Invoke-TestEngine`). It builds and imports `MaksIT.PodmanClientDotNet.PowerShell`, then runs domain scenarios against a real Podman API.

**Validated target:** [Podman](https://podman.io/) **5.4.0** (libpod path default `ApiVersion` = `v5.4.0`). Older engines that expose at least API `4.0.0` may work; Docker-compat path `v1.41` is not used (network endpoints reject it).

#### 1. Expose Podman on a Linux VM (e.g. Alma)

```bash
podman version   # expect 5.4.x for this repo's E2E baseline

# Bind all interfaces (not only 127.0.0.1)
podman system service --time=0 tcp://0.0.0.0:8080
```

On the VM:

```bash
curl -s http://127.0.0.1:8080/v5.4.0/_ping   # expect OK
ss -tlnp | grep 8080                        # expect 0.0.0.0:8080 (or LAN IP)
sudo firewall-cmd --add-port=8080/tcp --permanent && sudo firewall-cmd --reload
podman pull alpine:latest                   # scenarios need registry access
```

Optional systemd unit (lab):

```ini
[Unit]
Description=Podman API service
After=network.target

[Service]
ExecStart=/usr/bin/podman system service --time=0 tcp://0.0.0.0:8080
Restart=on-failure

[Install]
WantedBy=multi-user.target
```

Unauthenticated TCP API is lab-only — restrict firewall to your developer machine when possible.

#### 2. Run E2E from Windows

```powershell
Invoke-WebRequest http://<vm-ip>:8080/v5.4.0/_ping   # expect 200 / OK
$env:PODMAN_TEST_URL = "http://<vm-ip>:8080"        # must include http:// or https://
.\src\e2e-tests\Test-PodmanE2E.bat
# or:
pwsh -File .\src\e2e-tests\Test-PodmanE2E.ps1 -Scenario 'System','Images'
```

`PODMAN_TEST_URL` must be an absolute URI. Bare `host:port` causes `UriFormatException` in the .NET client.

Scenarios cover system, images, containers, exec, volumes, networks, pods, build, manifests, and generate (full PowerShell cmdlet surface). Filter with `-Scenario`.

#### Troubleshooting

1. **Pull fails** — VM cannot reach a registry; pre-pull `alpine:latest` on the VM.
2. **Container create/start fails** — confirm `podman run --rm alpine:latest echo ok` as the same user running `system service`.
3. **Attach/exec session failures** — hijack opens a second TCP connection to the same host:port; allow it in the firewall; do not put the API behind a proxy that strips `Upgrade: tcp`.
4. **pwsh / .NET 10** — binary module requires PowerShell hosted on .NET 10.
5. **`version is not supported` on networks** — use libpod path `v4.0.0+` (default `v5.4.0`), not Docker-compat `v1.41`.

### PowerShell module

```powershell
dotnet build src/PodmanClient.PowerShell/PodmanClient.PowerShell.csproj
Import-Module .\src\PodmanClient.PowerShell\bin\Debug\net10.0\MaksIT.PodmanClientDotNet.PowerShell.psd1 -Force
Connect-Podman -BaseAddress $env:PODMAN_TEST_URL
```

See [src/PodmanClient.PowerShell/README.md](src/PodmanClient.PowerShell/README.md).

## Commit message format

```text
(type): description
```

Types: `(feature):`, `(bugfix):`, `(refactor):`, `(perf):`, `(test):`, `(docs):`, `(build):`, `(ci):`, `(style):`, `(revert):`, `(chore):`.

- Lowercase description; no trailing period.

## Code style

- **.NET 10**, nullable reference types, implicit usings.
- **Root namespace**: `MaksIT.$(MSBuildProjectName)` in `PodmanClientDotNet.csproj`; omit `namespace` when it matches the root (client partials, abstractions).
- **MaksIT.Results** for API outcomes; **System.Text.Json** source generation via `PodmanJsonContext` for JSON serialization (AOT/trim-safe).
- File-scoped namespaces and same-line braces; **Models/** use nullable reference types (`string?`, `List<T>?`, …) for optional JSON fields.
- XML documentation on public types (DTOs, interfaces, entry types). Method-level docs on large interfaces are optional (`CS1591` suppressed).

## Pull requests

1. Build and unit tests pass; run E2E when changing the client or PowerShell surface if a Podman API is available.
2. Update **README.md** / **CHANGELOG.md** when behavior or public API changes.
3. If coverage changed: ensure **README.md** shields.io badge lines were updated by the test engine.
4. Keep diffs scoped.

## Versioning

[Semantic Versioning](https://semver.org): bump `Version` in `src/PodmanClient/PodmanClientDotNet.csproj` with **CHANGELOG.md** for releases. Use `utils\Invoke-ReleasePackage-Single.bat` when releasing.
