# Contributing to PodmanClient.DotNet

Thank you for contributing. This repo follows MaksIT conventions (see `AGENTS.md` and homelab `common/csharp` / `common/maksit-repo-maintenance` skills).

## Development setup

### Prerequisites

- .NET 10 SDK
- Git
- Optional: reachable Podman API for integration tests (`PODMAN_TEST_URL` or `PODMAN_INTEGRATION_URL`)

### Build

```bash
dotnet build src/PodmanClientDotNet.slnx
```

### Tests

**RepoUtils test engine** (coverage + badges):

```bash
utils/Invoke-TestEngine.bat
```

**Direct:**

```bash
dotnet test src/PodmanClientDotNet.Tests/PodmanClientDotNet.Tests.csproj
```

When coverage changes and the test engine runs **CoverageBadges**, commit updated SVGs under `assets/badges/` (cited in `README.md`).

## Commit message format

```text
(type): description
```

Types: `(feature):`, `(bugfix):`, `(refactor):`, `(perf):`, `(test):`, `(docs):`, `(build):`, `(ci):`, `(style):`, `(revert):`, `(chore):`.

- Lowercase description; no trailing period.

## Code style

- **.NET 10**, nullable reference types, implicit usings.
- **Root namespace**: `MaksIT.$(MSBuildProjectName)` in `PodmanClientDotNet.csproj`; omit `namespace` when it matches the root (client partials, abstractions).
- **MaksIT.Results** for API outcomes; **MaksIT.Core.Extensions** for JSON (`ToJson` / `ToObject<T>`).
- File-scoped namespaces and same-line braces; **Models/** use nullable reference types (`string?`, `List<T>?`, …) for optional JSON fields.
- XML documentation on public types (DTOs, interfaces, entry types). Method-level docs on large interfaces are optional (`CS1591` suppressed).
- Model layout helpers: `utils/tools/Polish-PodmanClientSources.ps1`, `utils/tools/Enable-ModelsNullable.ps1`.

## Pull requests

1. Build and tests pass.
2. Update **README.md** / **CHANGELOG.md** when behavior or public API changes.
3. Refresh **`assets/badges/*.svg`** when coverage badges change.
4. Keep diffs scoped.

## Versioning

[Semantic Versioning](https://semver.org): bump `Version` in `src/PodmanClient/PodmanClientDotNet.csproj` with **CHANGELOG.md** for releases. Use `utils/Invoke-ReleasePackage.bat` when releasing.
