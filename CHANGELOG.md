# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.3.0] - 2026-07-27

### Added

- **PowerShell module** `MaksIT.PodmanClientDotNet.PowerShell` wrapping the full `IPodmanClient` surface (`Connect-Podman` and domain cmdlets).
- **PowerShell E2E** harness under `src/e2e-tests/` (requires `PODMAN_TEST_URL`) covering system, images, containers, exec, volumes, networks, pods, build, manifests, and generate. Validated against **Podman 5.4.0**.

### Changed

- **Default `ApiVersion`**: `v5.4.0` (libpod path). Docker-compat `v1.41` is not used — network endpoints reject it.
- **Manifests (libpod v4+)**: create via `POST /libpod/manifests/{name}`; add via `PUT` modify body; push via `POST .../registry/{destination}` (replaces deprecated v3 `/create`, `/add`, `/push`).
- **Pod stats**: `GetPodsStatsAsync` returns `List<PodStatsDto>` (libpod array), not a dictionary wrapper.

### Fixed

- **Ping** (`/_ping`): treat Podman's plain-text `OK` body as success instead of JSON-deserializing it (which threw `JsonException`).
- **Mount container**: treat plain filesystem path body as `ContainerMountDto.Path`.
- **Wait container**: accept bare exit-code integer responses from libpod.
- **System DTOs** (`LibpodVersionDto`, `InfoDto`, `SystemDfDto`): align with live libpod JSON shapes (e.g. version `Platform` object, host `distribution` object, numeric memory fields).
- **Image DTOs**: `RepoTags`/`RepoDigests` as string arrays; image tree `{Tree}`; image changes as path/kind entries; delete/remove returns a single `ImageDeleteDto` object (not an array).
- **Prune APIs**: image/container/volume/pod prune return a list of `PruneReportEntryDto`; system prune returns `SystemPruneReportDto`.
- **Container list/inspect/stats/changes/mounted DTOs**: align with live libpod JSON (including multi-container stats wrapper).
- **Container inspect**: `Config.StopSignal` is a string (e.g. `SIGTERM`), not `Int64`.
- **Pod DTOs**: inspect/list `Containers` as object summaries.

### Removed

- C# xUnit live Integration tests (`Category=Integration`); replaced by PowerShell E2E scenarios.

## [1.2.1] - 2026-07-10

### Fixed

- **Inspect exec** deserialization: `ProcessConfig` is now typed as an object (`InspectExecProcessDto`) matching the Podman libpod API, instead of `string` (which threw `JsonException` when reading exit codes after exec).

## [1.2.0] - 2026-07-02

### Added

- **Native AOT and trimming compatibility** via `System.Text.Json` source generation (`PodmanJsonContext` with `[JsonSerializable]` for all library DTOs and request models).
- `<IsAotCompatible>true</IsAotCompatible>` on the package project to surface trim/AOT Roslyn analyzers at build time.

### Changed

- Internal JSON serialization/deserialization now uses explicit `JsonTypeInfo<T>` (`JsonSerializer.Serialize` / `Deserialize`) instead of reflection-based `MaksIT.Core.Extensions` `ToJson()` / `ToObject<T>()`.
- HTTP helpers (`GetJsonAsync`, `PostJsonAsync`, `PostJsonWithoutBodyAsync`, `PostLibpodAsync`, `DeleteJsonAsync`) and `PodmanProgressSession<T>` updated to thread source-generated type metadata through all call sites.
- `PodmanJsonContext` uses `PropertyNameCaseInsensitive = true` so Podman lowercase JSON keys map to PascalCase DTO properties.

### Removed

- **MaksIT.Core** package reference (JSON helpers were its only use in this library); consumers no longer pull **MaksIT.Core** transitively from **PodmanClient.DotNet**.

### Thanks

- [@bbartels](https://github.com/bbartels) (Benjamin Bartels) for the Native AOT / trim compatibility contribution ([#1](https://github.com/MAKS-IT-COM/podman-client-dotnet/pull/1)).

## [1.1.1] - 2026-06-28

### Changed

- Updated **MaksIT.Core** (1.6.8), **MaksIT.Results** (2.0.3), and **Microsoft.Extensions.*** (10.0.9) package references.
- Updated test dependencies: **Microsoft.NET.Test.Sdk** (18.7.0), **Microsoft.Extensions.Logging.Console** (10.0.9).

## [1.1.0] - 2026-06-04

### Added

- Full **Libpod API** coverage (~86 endpoints) via domain interfaces: `IPodmanSystemClient`, `IPodmanContainersClient`, `IPodmanImagesClient`, `IPodmanVolumesClient`, `IPodmanNetworksClient`, `IPodmanPodsClient`, `IPodmanExecClient`, `IPodmanBuildClient`, `IPodmanManifestsClient`, `IPodmanGenerateClient` (composed by `IPodmanClient`).
- Typed API responses under `Dtos/` (`*Dto` suffix); request/spec payloads remain in `Models/`.
- **Streaming APIs:** `AttachContainerSessionAsync`, `StartExecSessionAsync` (`IPodmanAttachSession`), `PullImageWithProgressAsync`, `BuildImageWithProgressAsync` (`IPodmanProgressSession<T>`), plus hijack connection and multiplex protocol internals.
- Shared HTTP helpers in `PodmanClient.Http.cs` and NDJSON stream handling in `PodmanNdjsonStreams`.
- `IPodmanClientConfiguration`, `AddPodmanClient` (`IHttpClientFactory` / `AddHttpClient`); host apps supply their own configuration implementation.
- Unit tests for streaming, NDJSON, and hijack mock server; integration tests tagged `Category=Integration` (skip without `PODMAN_TEST_URL`).
- `CHANGELOG.md`, `CONTRIBUTING.md`, coverage badge assets, and `utils/` (RepoUtils test/release engines).

### Changed

- Target framework upgraded to **.NET 10** (`net10.0`).
- API methods return **MaksIT.Results** `Result` / `Result<T>` instead of throwing on Podman HTTP errors.
- Added **MaksIT.Core** and **MaksIT.Results** dependencies; removed local `Extensions` (`ToJson` / `ToObject`) in favor of `MaksIT.Core.Extensions`.
- `PodmanClient` split into partials (`PodmanClient.Http.cs`, `PodmanClient.Containers.Api.cs`, etc.); solution file migrated to `PodmanClientDotNet.slnx`.
- Package metadata, Source Link, symbol packages, and documentation generation aligned with [maksit-core](https://github.com/MAKS-IT-COM/maksit-core) standards.
- Registry auth (`X-Registry-Auth`) applied per HTTP request instead of mutating shared `HttpClient.DefaultRequestHeaders`.
- Replaced legacy `src/Release-NuGetPackage.*` scripts and `.nuspec` with SDK-style pack + `utils/` release tooling.

### Fixed

- Pull, push, and build endpoints consume NDJSON progress streams correctly; `BuildImageAsync` no longer deserializes a multi-line build stream as a single JSON object.
- Attach hijack requests include the `tty` query parameter.
- Manual `PodmanClient` constructor preserves caller-configured `HttpClient.Timeout` (no longer truncated via integer minutes cast).

### Removed

- Concrete `PodmanClientConfiguration` type from the library package.
- Monolithic `PodmanClientContainer.cs`, `PodmanClientExec.cs`, and `PodmanClientImage.cs` (superseded by partials).

### Breaking

- Method return types changed from `Task` / `Task<T?>` to `Result` / `Result<T?>`.
- Response types moved to `Dtos/`; update usings from `Models.*` response classes.
- Removed `PodmanClientConfiguration`; bind `IPodmanClientConfiguration` with a host-owned options class.
- Prefer `IPodmanClient` and `AddPodmanClient` for DI; manual `PodmanClient` constructors remain for tests and simple hosts.

## [1.0.4] - 2024-08-18

### Added

- Integration tests for container lifecycle, exec, and image pull/tag.

### Fixed

- Empty-string JSON parse issue in HTTP response handling.

### Changed

- Package readme and repository documentation updates.

## [1.0.2] - 2024-08-17

### Added

- Initial **PodmanClient.DotNet** library on **.NET 8** (`net8.0`).
- Container operations: create, start, stop, delete, archive copy.
- Exec operations: create, start, inspect.
- Image operations: pull, tag.
- NuGet packaging (`.nuspec`, `Release-NuGetPackage` scripts) and README.
