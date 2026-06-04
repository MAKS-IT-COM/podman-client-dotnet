using MaksIT.PodmanClientDotNet.Tests.Archives;

namespace MaksIT.PodmanClientDotNet.Tests;

[Trait("Category", "Integration")]
public class PodmanClientContainersTests {
  private readonly IPodmanClient _client = PodmanClientTestFixture.CreateClient();

  #region Success Cases
  [Fact]
  public async Task PodmanClient_ContainerLifecycle_Success() {
    string containerName = $"podman-client-test-{Guid.NewGuid()}";
    string image = "alpine:latest";

    await PullImageAsync(image);
    var containerId = await CreateContainerAsync(containerName, image);
    await StartContainerAsync(containerId);
    await StopContainerAsync(containerId);
    await ForceDeleteContainerAsync(containerId);
  }

  [Fact]
  public async Task CopyFilesToContainer_Success() {
    string containerName = $"podman-client-test-{Guid.NewGuid()}";
    string image = "alpine:latest";
    string pathInContainer = "/podman-test-copy";
    string tempFolderPath = CreateTemporaryFolderWithFiles();

    try {
      await PullImageAsync(image);
      var containerId = await CreateContainerAsync(containerName, image);
      await StartContainerAsync(containerId);

      using var tarStream = CreateTarStream(tempFolderPath);
      await CopyToContainerAsync(containerId, tarStream, pathInContainer);

      await StopContainerAsync(containerId);
      await ForceDeleteContainerAsync(containerId);
    }
    finally {
      if (Directory.Exists(tempFolderPath))
        Directory.Delete(tempFolderPath, true);
    }
  }
  #endregion

  #region Helper Methods
  private async Task PullImageAsync(string image) {
    var result = await _client.PullImageAsync(image);
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private async Task<string> CreateContainerAsync(string containerName, string image) {
    var result = await _client.CreateContainerAsync(
      name: containerName,
      image: image,
      command: new List<string> { "sh", "-c", "sleep infinity" });

    string? containerId = null;
    PodmanClientTestFixture.AssertSuccess(result, value => {
      Assert.NotNull(value);
      Assert.False(string.IsNullOrEmpty(value!.Id));
      containerId = value.Id;
    });

    return containerId!;
  }

  private async Task StartContainerAsync(string containerId) {
    var result = await _client.StartContainerAsync(containerId);
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private async Task StopContainerAsync(string containerId) {
    var result = await _client.StopContainerAsync(containerId);
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private async Task ForceDeleteContainerAsync(string containerId) {
    var result = await _client.ForceDeleteContainerAsync(containerId);
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private async Task CopyToContainerAsync(string containerId, Stream tarStream, string path) {
    var result = await _client.ExtractArchiveToContainerAsync(containerId, tarStream, path);
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private static string CreateTemporaryFolderWithFiles() {
    string tempFolder = Path.Combine(Path.GetTempPath(), $"podman-test-{Guid.NewGuid()}");
    Directory.CreateDirectory(tempFolder);

    for (int i = 0; i < 5; i++)
      File.WriteAllText(Path.Combine(tempFolder, $"test-file-{i}.txt"), $"This is test file {i}");

    return tempFolder;
  }

  private static Stream CreateTarStream(string folderPath) {
    var memoryStream = new MemoryStream();
    Tar.CreateTarFromDirectory(folderPath, memoryStream);
    memoryStream.Seek(0, SeekOrigin.Begin);
    return memoryStream;
  }
  #endregion

  #region Fail Cases
  [Fact]
  public async Task StartContainerAsync_Should_HandleErrors() {
    var result = await _client.StartContainerAsync("invalid-container-id");
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task StopContainerAsync_Should_HandleErrors() {
    var result = await _client.StopContainerAsync("invalid-container-id");
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task ForceDeleteContainerAsync_Should_HandleErrors() {
    var result = await _client.ForceDeleteContainerAsync("invalid-container-id");
    PodmanClientTestFixture.AssertFailure(result);
  }
  #endregion
}
