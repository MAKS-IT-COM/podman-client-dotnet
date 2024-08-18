using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace MaksIT.PodmanClientDotNet.Tests {
  public class PodmanClientExecTests {
    private readonly PodmanClient _client;

    public PodmanClientExecTests() {
      var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
      var logger = loggerFactory.CreateLogger<PodmanClient>();

      _client = new PodmanClient(logger, "http://wks0002.corp.maks-it.com:8080", 60);
    }

    #region Success Cases

    [Fact]
    public async Task Full_ContainerLifecycle_With_Exec_Should_Succeed() {
      // Arrange
      string containerName = $"podman-client-test-{Guid.NewGuid()}";
      string image = "alpine:latest";

      // Act & Assert
      // 1. Pull the image
      await PullImageAsync(image);

      // 2. Create the container with sleep infinity command
      var containerId = await CreateContainerAsync(containerName, image);

      // 3. Start the container
      await StartContainerAsync(containerId);

      // 4. Execute a command in the container to install a package (e.g., "apk add curl")
      var execId = await CreateExecAsync(containerName, new[] { "apk", "add", "--no-cache", "curl" });
      await StartExecAsync(execId);

      // 5. Stop the container
      await StopContainerAsync(containerId);

      // 6. Delete the container
      await ForceDeleteContainerAsync(containerId);
    }

    #endregion

    #region Helper Methods

    private async Task PullImageAsync(string image) {
      var exception = await Record.ExceptionAsync(() => _client.PullImageAsync(image));
      Assert.Null(exception); // Expect no exceptions if the pull was successful
    }

    private async Task<string> CreateContainerAsync(string containerName, string image) {
      var createResponse = await _client.CreateContainerAsync(
        name: containerName,
        image: image,
        command: new List<string> {
          "sh", "-c",
          "sleep infinity"
        });
      Assert.NotNull(createResponse);
      Assert.False(string.IsNullOrEmpty(createResponse.Id)); // Ensure a valid container ID is returned
      return createResponse.Id;
    }

    private async Task StartContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.StartContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was started successfully
    }

    private async Task<string> CreateExecAsync(string containerName, string[] cmd) {
      var execResponse = await _client.CreateExecAsync(containerName, cmd);
      Assert.NotNull(execResponse);
      Assert.False(string.IsNullOrEmpty(execResponse.Id)); // Ensure a valid exec ID is returned
      return execResponse.Id;
    }

    private async Task StartExecAsync(string execId) {
      var exception = await Record.ExceptionAsync(() => _client.StartExecAsync(execId));
      Assert.Null(exception); // Expect no exceptions if the exec command was started successfully
    }

    private async Task StopContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.StopContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was stopped successfully
    }

    private async Task ForceDeleteContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.ForceDeleteContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was deleted successfully
    }

    #endregion

    #region Fail Cases

    [Fact]
    public async Task PullImageAsync_Should_HandleErrors() {
      // Arrange
      string invalidImageReference = "invalidimage:latest"; // Intentionally wrong image

      // Act
      var exception = await Record.ExceptionAsync(() => _client.PullImageAsync(invalidImageReference));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task CreateContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidImage = "invalidimage:latest"; // Intentionally wrong image
      string containerName = "test-container";

      // Act
      var exception = await Record.ExceptionAsync(() => _client.CreateContainerAsync(containerName, invalidImage, new List<string> { "sh", "-c", "sleep infinity" }));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task StartContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidContainerId = "invalid-container-id"; // Intentionally wrong container ID

      // Act
      var exception = await Record.ExceptionAsync(() => _client.StartContainerAsync(invalidContainerId));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task CreateExecAsync_Should_HandleErrors() {
      // Arrange
      string containerName = "invalid-container"; // Intentionally wrong container name
      var cmd = new[] { "apk", "add", "--no-cache", "curl" };

      // Act
      var exception = await Record.ExceptionAsync(() => _client.CreateExecAsync(containerName, cmd));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task StartExecAsync_Should_HandleErrors() {
      // Arrange
      string invalidExecId = "invalid-exec-id"; // Intentionally wrong exec ID

      // Act
      var exception = await Record.ExceptionAsync(() => _client.StartExecAsync(invalidExecId));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task StopContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidContainerId = "invalid-container-id"; // Intentionally wrong container ID

      // Act
      var exception = await Record.ExceptionAsync(() => _client.StopContainerAsync(invalidContainerId));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task ForceDeleteContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidContainerId = "invalid-container-id"; // Intentionally wrong container ID

      // Act
      var exception = await Record.ExceptionAsync(() => _client.ForceDeleteContainerAsync(invalidContainerId));

      // Assert
      Assert.NotNull(exception);
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    #endregion 
  }
}
