# PodmanClientDotNet

## Description

`PodmanClientDotNet` is a .NET library designed to provide seamless interaction with the Podman API, allowing developers to manage and control containers directly from their .NET applications. This client library wraps the Podman API endpoints, offering a .NET-friendly interface to perform common container operations such as creating, starting, stopping, deleting containers, and more.

## Purpose

The primary goal of `PodmanClientDotNet` is to simplify the integration of Podman into .NET applications by providing a comprehensive, easy-to-use client library. Whether you're managing container lifecycles, executing commands inside containers, or manipulating container images, this library allows developers to interface with Podman using the familiar .NET development environment.

## Key Features

- **Container Management:** Create, start, stop, and delete containers with straightforward methods.
- **Image Operations:** Pull, tag, and manage images using the Podman API.
- **Exec Support:** Execute commands inside running containers, with support for attaching input/output streams.
- **Volume and Network Management:** Manage container volumes and networks as needed.
- **Streamlined Error Handling:** Provides detailed error handling, with informative responses based on HTTP status codes.

## Usage Example

```csharp
// Initialize the PodmanClient with base URL and timeout
var podmanClient = new PodmanClient("http://localhost:8080", 5);

// Create a new container
var createResponse = await podmanClient.CreateContainerAsync(
    name: "my-container",
    image: "alpine:latest",
    command: new List<string> { "/bin/sh" },
    env: new Dictionary<string, string> { { "ENV_VAR", "value" } },
    remove: true
);

// Start the container
await podmanClient.StartContainerAsync(createResponse.Id);

// Execute a command inside the container
var execResponse = await podmanClient.CreateExecAsync(createResponse.Id, new[] { "echo", "Hello, World!" });
await podmanClient.StartExecAsync(execResponse.Id);
```

## Installation

To include `PodmanClientDotNet` in your .NET project, you can add the package via NuGet:

```shell
dotnet add package PodmanClientDotNet
```

## Documentation

For detailed documentation on each method, including parameter descriptions and example usage, please refer to the official documentation (link to be provided).

## Contribution

Contributions to this project are welcome! Please fork the repository and submit a pull request with your changes. If you encounter any issues or have feature requests, feel free to open an issue on GitHub.

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