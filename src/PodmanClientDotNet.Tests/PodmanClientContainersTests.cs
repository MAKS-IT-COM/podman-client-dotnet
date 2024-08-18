using Microsoft.Extensions.Logging;
using Xunit;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Tests {
  public class PodmanClientContainersTests {
    private readonly PodmanClient _client;

    public PodmanClientContainersTests() {
      // Initialize the logger
      var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
      var logger = loggerFactory.CreateLogger<PodmanClient>();

      // Initialize PodmanClient with real HttpClient
      _client = new PodmanClient(logger, "http://wks0002.corp.maks-it.com:8080", 60);
    }

    #region Success
    [Fact]
    public async Task PodmanClient_ContainerLifecycle_Success() {
      // Arrange
      string containerName = "test-container";
      string image = "alpine:latest";

      // Act & Assert
      await PullImageAsync(image);
      var containerId = await CreateContainerAsync(containerName, image);
      await StartContainerAsync(containerId);
      await StopContainerAsync(containerId);
      await ForceDeleteContainerAsync(containerId);
    }

    private async Task PullImageAsync(string image) {
      // Implement the logic to pull the image
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

    private async Task StopContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.StopContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was stopped successfully
    }

    private async Task ForceDeleteContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.ForceDeleteContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was deleted successfully
    }
    #endregion

    #region Fail
    [Fact]
    public async Task StartContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidContainerId = "invalid-container-id";

      // Act
      var exception = await Record.ExceptionAsync(() => _client.StartContainerAsync(invalidContainerId));

      // Assert
      Assert.NotNull(exception); // Expect an exception due to invalid container ID
    }

    [Fact]
    public async Task StopContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidContainerId = "invalid-container-id";

      // Act
      var exception = await Record.ExceptionAsync(() => _client.StopContainerAsync(invalidContainerId));

      // Assert
      Assert.NotNull(exception); // Expect an exception due to invalid container ID
    }

    [Fact]
    public async Task ForceDeleteContainerAsync_Should_HandleErrors() {
      // Arrange
      string invalidContainerId = "invalid-container-id";

      // Act
      var exception = await Record.ExceptionAsync(() => _client.ForceDeleteContainerAsync(invalidContainerId));

      // Assert
      Assert.NotNull(exception); // Expect an exception due to invalid container ID
    }
    #endregion
  }
}
