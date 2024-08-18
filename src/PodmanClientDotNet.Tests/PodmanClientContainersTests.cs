using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Tests.Archives;

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

    #region Success Cases
    [Fact]
    public async Task PodmanClient_ContainerLifecycle_Success() {
      // Arrange
      string containerName = $"podman-client-test-{Guid.NewGuid()}";
      string image = "alpine:latest";

      // Act & Assert
      await PullImageAsync(image);
      var containerId = await CreateContainerAsync(containerName, image);
      await StartContainerAsync(containerId);
      await StopContainerAsync(containerId);
      await ForceDeleteContainerAsync(containerId);
    }

    [Fact]
    public async Task CopyFilesToContainer_Success() {
      // Arrange
      string containerName = $"podman-client-test-{Guid.NewGuid()}";
      string image = "alpine:latest";
      string pathInContainer = "/podman-test-copy";

      // Create temporary folder with random files
      string tempFolderPath = CreateTemporaryFolderWithFiles();

      try {
        // Act
        await PullImageAsync(image);
        var containerId = await CreateContainerAsync(containerName, image);
        await StartContainerAsync(containerId);

        // Archive the folder and copy to container
        using (var tarStream = CreateTarStream(tempFolderPath)) {
          await CopyToContainerAsync(containerId, tarStream, pathInContainer);
        }

        // Stop and delete the container
        await StopContainerAsync(containerId);
        await ForceDeleteContainerAsync(containerId);
      }
      finally {
        // Cleanup: Delete temporary folder
        if (Directory.Exists(tempFolderPath)) {
          Directory.Delete(tempFolderPath, true);
        }
      }
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

    private async Task StopContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.StopContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was stopped successfully
    }

    private async Task ForceDeleteContainerAsync(string containerId) {
      var exception = await Record.ExceptionAsync(() => _client.ForceDeleteContainerAsync(containerId));
      Assert.Null(exception); // Expect no exceptions if the container was deleted successfully
    }

    private async Task CopyToContainerAsync(string containerId, Stream tarStream, string path) {
      var exception = await Record.ExceptionAsync(() => _client.ExtractArchiveToContainerAsync(containerId, tarStream, path));
      Assert.Null(exception); // Expect no exceptions if the copy was successful
    }

    private string CreateTemporaryFolderWithFiles() {
      string tempFolder = Path.Combine(Path.GetTempPath(), $"podman-test-{Guid.NewGuid()}");
      Directory.CreateDirectory(tempFolder);

      // Create some random files
      for (int i = 0; i < 5; i++) {
        File.WriteAllText(Path.Combine(tempFolder, $"test-file-{i}.txt"), $"This is test file {i}");
      }

      return tempFolder;
    }

    private Stream CreateTarStream(string folderPath) {
      var memoryStream = new MemoryStream();
      Tar.CreateTarFromDirectory(folderPath, memoryStream);
      memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position for reading
      return memoryStream;
    }
    #endregion

    #region Fail Cases
    [Fact]
    public async Task StartContainerAsync_Should_HandleErrors() {
      string invalidContainerId = "invalid-container-id";
      var exception = await Record.ExceptionAsync(() => _client.StartContainerAsync(invalidContainerId));
      Assert.NotNull(exception); // Expect an exception due to invalid container ID
    }

    [Fact]
    public async Task StopContainerAsync_Should_HandleErrors() {
      string invalidContainerId = "invalid-container-id";
      var exception = await Record.ExceptionAsync(() => _client.StopContainerAsync(invalidContainerId));
      Assert.NotNull(exception); // Expect an exception due to invalid container ID
    }

    [Fact]
    public async Task ForceDeleteContainerAsync_Should_HandleErrors() {
      string invalidContainerId = "invalid-container-id";
      var exception = await Record.ExceptionAsync(() => _client.ForceDeleteContainerAsync(invalidContainerId));
      Assert.NotNull(exception); // Expect an exception due to invalid container ID
    }
    #endregion
  }
}
