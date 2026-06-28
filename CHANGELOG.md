# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
