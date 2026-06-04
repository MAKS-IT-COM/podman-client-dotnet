namespace MaksIT.PodmanClientDotNet.Tests;

[Trait("Category", "Integration")]
public class PodmanClientExecTests {
  private readonly IPodmanClient _client = PodmanClientTestFixture.CreateClient();

  #region Success Cases
  [Fact]
  public async Task Full_ContainerLifecycle_With_Exec_Should_Succeed() {
    string containerName = $"podman-client-test-{Guid.NewGuid()}";
    string image = "alpine:latest";

    await PullImageAsync(image);
    var containerId = await CreateContainerAsync(containerName, image);
    await StartContainerAsync(containerId);

    var execId = await CreateExecAsync(containerName, new[] { "apk", "add", "--no-cache", "curl" });
    await StartExecAsync(execId);

    await StopContainerAsync(containerId);
    await ForceDeleteContainerAsync(containerId);
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

  private async Task<string> CreateExecAsync(string containerName, string[] cmd) {
    var result = await _client.CreateExecAsync(containerName, cmd);

    string? execId = null;
    PodmanClientTestFixture.AssertSuccess(result, value => {
      Assert.NotNull(value);
      Assert.False(string.IsNullOrEmpty(value!.Id));
      execId = value.Id;
    });

    return execId!;
  }

  private async Task StartExecAsync(string execId) {
    var result = await _client.StartExecAsync(execId);
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
  #endregion

  #region Fail Cases
  [Fact]
  public async Task PullImageAsync_Should_HandleErrors() {
    var result = await _client.PullImageAsync("invalidimage:latest");
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task CreateContainerAsync_Should_HandleErrors() {
    var result = await _client.CreateContainerAsync(
      "test-container",
      "invalidimage:latest",
      new List<string> { "sh", "-c", "sleep infinity" });
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task StartContainerAsync_Should_HandleErrors() {
    var result = await _client.StartContainerAsync("invalid-container-id");
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task CreateExecAsync_Should_HandleErrors() {
    var result = await _client.CreateExecAsync("invalid-container", new[] { "apk", "add", "--no-cache", "curl" });
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task StartExecAsync_Should_HandleErrors() {
    var result = await _client.StartExecAsync("invalid-exec-id");
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
