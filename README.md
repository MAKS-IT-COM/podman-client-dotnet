# PodmanClient.DotNet

![Line Coverage](assets/badges/coverage-lines.svg) ![Branch Coverage](assets/badges/coverage-branches.svg) ![Method Coverage](assets/badges/coverage-methods.svg)

## Description

`PodmanClient.DotNet` is a .NET library designed to provide seamless interaction with the Podman API, allowing developers to manage and control containers directly from their .NET applications. This client library wraps the Podman API endpoints, offering a .NET-friendly interface to perform common container operations such as creating, starting, stopping, deleting containers, and more.

## Purpose

The primary goal of `PodmanClient.DotNet` is to simplify the integration of Podman into .NET applications by providing a comprehensive, easy-to-use client library. Whether you're managing container lifecycles, executing commands inside containers, or manipulating container images, this library allows developers to interface with Podman using the familiar .NET development environment.

## Key Features

- **Full Libpod API surface:** System, containers, images, volumes, networks, pods, exec, build, manifests, and generate/play/kube (see `IPodmanClient` and domain interfaces under `Abstractions/`).
- **Domain-oriented API:** `IPodmanContainersClient`, `IPodmanImagesClient`, `IPodmanVolumesClient`, and related interfaces; `IPodmanClient` composes them all.
- **Structured results:** API methods return `MaksIT.Results.Result` / `Result<T>` instead of throwing on Podman HTTP errors.
- **Dependency injection:** Register with `AddHttpClient` via `AddPodmanClient` and inject `IPodmanClient`.
- **Logging Support:** Integrated logging support via `Microsoft.Extensions.Logging` for better observability.
- **Streaming:** Full-duplex attach/exec sessions (`IPodmanAttachSession`) and NDJSON progress for pull/build (`IPodmanProgressSession<T>`).

## Installation

To include `PodmanClient.DotNet` in your .NET project, you can add the package via NuGet:

```shell
dotnet add package PodmanClient.DotNet
```

## Usage Examples

### Dependency injection (recommended)

`appsettings.json`:

```json
{
  "PodmanClient": {
    "ServerUrl": "http://localhost:8080",
    "ApiVersion": "v1.41",
    "TimeoutMinutes": 5
  }
}
```

```csharp
using MaksIT.PodmanClientDotNet;
using MaksIT.PodmanClientDotNet.Extensions;

// Host-owned options type (not shipped in this package)
public sealed class PodmanClientOptions : IPodmanClientConfiguration {
  public string ServerUrl { get; set; } = string.Empty;
  public string ApiVersion { get; set; } = "v1.41";
  public int TimeoutMinutes { get; set; } = 60;
}

var podmanConfiguration = builder.Configuration
  .GetSection(IPodmanClientConfiguration.SectionName)
  .Get<PodmanClientOptions>()
  ?? throw new InvalidOperationException("PodmanClient configuration is missing.");

builder.Services.AddPodmanClient(podmanConfiguration);

// Inject IPodmanClient where needed
public sealed class ContainerService(IPodmanClient podman) {
  public async Task<Result> RunAsync() {
    var create = await podman.CreateContainerAsync("my-container", "alpine:latest");
    if (!create.IsSuccess)
      return create.ToResult();

    return await podman.StartContainerAsync(create.Value!.Id);
  }
}
```

### Manual construction

```csharp
using Microsoft.Extensions.Logging;
using MaksIT.PodmanClientDotNet;

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PodmanClient>();
IPodmanClient podmanClient = new PodmanClient(logger, "http://localhost:8080", 5);
```

### Create and Start a Container

```csharp
var createResult = await podmanClient.CreateContainerAsync(
    name: "my-container",
    image: "alpine:latest",
    command: new List<string> { "/bin/sh" },
    env: new Dictionary<string, string> { { "ENV_VAR", "value" } },
    remove: true
);

if (!createResult.IsSuccess)
  return createResult.ToActionResult(); // in ASP.NET Core controllers

await podmanClient.StartContainerAsync(createResult.Value!.Id);
```

### Execute a Command in a Container

```csharp
var execResult = await podmanClient.CreateExecAsync(createResult.Value!.Id, new[] { "echo", "Hello, World!" });
if (execResult.IsSuccess)
  await podmanClient.StartExecAsync(execResult.Value!.Id);
```

### Pull an Image

```csharp
var pullResult = await podmanClient.PullImageAsync("alpine:latest");
```

### Tag an Image

```csharp
await podmanClient.TagImageAsync("alpine:latest", "myrepo", "mytag");
```

### Full-duplex attach (container or exec)

Use session APIs for multiplexed stdin/stdout/stderr (or raw TTY) instead of a one-shot `Stream`:

```csharp
var attach = await podmanClient.AttachContainerSessionAsync(
  containerId, stream: true, stdout: true, stderr: true, stdin: true, tty: false);
if (!attach.IsSuccess) return;

await using var session = attach.Value!;
while (await session.ReadFrameAsync() is { } frame)
  Console.Write(frame.StreamType + ": " + Encoding.UTF8.GetString(frame.Data));

await session.WriteStdinAsync("input"u8.ToArray());
await session.CloseWriteAsync();
```

Exec hijack: `CreateExecAsync` → `StartExecSessionAsync(execId, tty: false)`.

Pull/build progress (NDJSON): `PullImageWithProgressAsync` / `BuildImageWithProgressAsync` return `IPodmanProgressSession<T>`; enumerate with `await foreach (var line in session.ReadProgressAsync())`.

## Available Methods

### `IPodmanClient`

Register with `AddPodmanClient` or construct `PodmanClient` manually. Methods return `Result` / `Result<T>` from **MaksIT.Results**.

| Interface | Coverage |
|-----------|----------|
| `IPodmanSystemClient` | ping, version, info, events, disk usage, system prune |
| `IPodmanContainersClient` | create, list, inspect, lifecycle, logs, stats, archive, attach, commit, checkpoint, prune, … |
| `IPodmanImagesClient` | pull, push, list, inspect, tag, untag, search, load, import, export, prune, … |
| `IPodmanVolumesClient` | create, list, inspect, delete, prune |
| `IPodmanNetworksClient` | create, list, inspect, delete, connect, disconnect |
| `IPodmanPodsClient` | create, list, inspect, lifecycle, stats, prune, … |
| `IPodmanExecClient` | create, start, resize, inspect |
| `IPodmanBuildClient` | `BuildImageAsync` |
| `IPodmanManifestsClient` | create, inspect, add, push, delete |
| `IPodmanGenerateClient` | systemd unit, kube yaml, play kube |

API responses are typed under `Dtos/` (for example `ContainerInspectDto`, `ImageInspectDto`, `InfoDto`). Request/spec payloads remain in `Models/`.

## Tests

Unit tests cover multiplex framing, attach sessions, NDJSON progress, and a local hijack mock server. Integration tests require a reachable Podman API:

```shell
$env:PODMAN_TEST_URL = "http://localhost:8080"
dotnet test src/PodmanClientDotNet.Tests/PodmanClientDotNet.Tests.csproj
```

Without `PODMAN_TEST_URL` (or `PODMAN_INTEGRATION_URL`), integration tests are skipped automatically. Filter them in CI with `--filter "Category!=Integration"`.

**Note:** Full-duplex attach/exec sessions use a raw TCP hijack connection and do not flow through `HttpClient` delegating handlers (proxy, client certificates, etc.). Configure network access accordingly.

## Documentation (TODO: Agile)

For detailed documentation on each method, including parameter descriptions and example usage, please refer to the official documentation (link to be provided).

## Contribution

Contributions are welcome. See [CONTRIBUTING.md](CONTRIBUTING.md) for build/test commands, commit format, and PR expectations. Open an issue on GitHub for bugs or feature requests.

## License

This project is licensed under the MIT License. See [LICENSE.md](LICENSE.md).

---

### MIT License

```
MIT License

Copyright (c) 2024 Maksym Sadovnychyy (MAKS-IT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Contact

- **Email**: [maksym.sadovnychyy@gmail.com](mailto:maksym.sadovnychyy@gmail.com)
- **Reddit**: [PodmanClient.DotNet: A .NET Library for Streamlined Podman API Integration](https://www.reddit.com/r/MaksIT/comments/1evel9z/podmanclientdotnet_a_net_library_for_streamlined/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button)