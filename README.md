# PodmanClient.DotNet

## Description

`PodmanClient.DotNet` is a .NET library designed to provide seamless interaction with the Podman API, allowing developers to manage and control containers directly from their .NET applications. This client library wraps the Podman API endpoints, offering a .NET-friendly interface to perform common container operations such as creating, starting, stopping, deleting containers, and more.

## Purpose

The primary goal of `PodmanClient.DotNet` is to simplify the integration of Podman into .NET applications by providing a comprehensive, easy-to-use client library. Whether you're managing container lifecycles, executing commands inside containers, or manipulating container images, this library allows developers to interface with Podman using the familiar .NET development environment.

## Key Features

- **Container Management:** Create, start, stop, and delete containers with straightforward methods.
- **Image Operations:** Pull, tag, and manage images using the Podman API.
- **Exec Support:** Execute commands inside running containers, with support for attaching input/output streams.
- **Volume and Network Management:** Manage container volumes and networks as needed.
- **Streamlined Error Handling:** Provides detailed error handling, with informative responses based on HTTP status codes.
- **Customizable HTTP Client:** Easily configure and inject your own `HttpClient` instance for extended control and customization.
- **Logging Support:** Integrated logging support via `Microsoft.Extensions.Logging` for better observability.

## Installation

To include `PodmanClient.DotNet` in your .NET project, you can add the package via NuGet:

```shell
dotnet add package PodmanClient.DotNet
```

## Usage Examples

### Initialize the PodmanClient

```csharp
using Microsoft.Extensions.Logging;
using MaksIT.PodmanClient.DotNet;

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PodmanClient>();
var podmanClient = new PodmanClient(logger, "http://localhost:8080", 5);
```

### Create and Start a Container

```csharp
var createResponse = await podmanClient.CreateContainerAsync(
    name: "my-container",
    image: "alpine:latest",
    command: new List<string> { "/bin/sh" },
    env: new Dictionary<string, string> { { "ENV_VAR", "value" } },
    remove: true
);

await podmanClient.StartContainerAsync(createResponse.Id);
```

### Execute a Command in a Container

```csharp
var execResponse = await podmanClient.CreateExecAsync(createResponse.Id, new[] { "echo", "Hello, World!" });
await podmanClient.StartExecAsync(execResponse.Id);
```

### Pull an Image

```csharp
await podmanClient.PullImageAsync("alpine:latest");
```

### Tag an Image

```csharp
await podmanClient.TagImageAsync("alpine:latest", "myrepo", "mytag");
```

## Available Methods

### `PodmanClient`

- **Container Management**
  - `Task<CreateContainerResponse> CreateContainerAsync(...)`: Creates a new container.
  - `Task StartContainerAsync(string containerId, string detachKeys = "ctrl-p,ctrl-q")`: Starts an existing container.
  - `Task StopContainerAsync(string containerId, int timeout = 10, bool ignoreAlreadyStopped = false)`: Stops a running container.
  - `Task DeleteContainerAsync(string containerId, bool depend = false, bool ignore = false, int timeout = 10)`: Deletes a container.
  - `Task ForceDeleteContainerAsync(string containerId, bool deleteVolumes = false, int timeout = 10)`: Forcefully deletes a container, optionally removing associated volumes.

- **Exec Management**
  - `Task<CreateExecResponse> CreateExecAsync(...)`: Creates an exec instance in a running container.
  - `Task StartExecAsync(string execId, bool detach = false, bool tty = false, int? height = null, int? width = null)`: Starts an exec instance.
  - `Task<InspectExecResponse?> InspectExecAsync(string execId)`: Inspects an exec instance to retrieve its details.

- **Image Operations**
  - `Task PullImageAsync(...)`: Pulls an image from a container registry.
  - `Task TagImageAsync(string image, string repo, string tag)`: Tags an existing image with a new repository and tag.

- **File Operations**
  - `Task ExtractArchiveToContainerAsync(string containerId, Stream tarStream, string path, bool pause = true)`: Extracts files from a tar stream into a container.

## Documentation (TODO: Agile)

For detailed documentation on each method, including parameter descriptions and example usage, please refer to the official documentation (link to be provided).

## Contribution

Contributions to this project are welcome! Please fork the repository and submit a pull request with your changes. If you encounter any issues or have feature requests, feel free to open an issue on GitHub.

## Contact

If you have any questions or need further assistance, feel free to reach out:

- **Email**: [maksym.sadovnychyy@gmail.com](mailto:maksym.sadovnychyy@gmail.com)
- **Reddit**: [PodmanClient.DotNet: A .NET Library for Streamlined Podman API Integration](https://www.reddit.com/r/MaksIT/comments/1evel9z/podmanclientdotnet_a_net_library_for_streamlined/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button)

## License

This project is licensed under the MIT License. See the full license text below.

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

For any questions or inquiries, please reach out via GitHub or [email](mailto:maksym.sadovnychyy@gmail.com).