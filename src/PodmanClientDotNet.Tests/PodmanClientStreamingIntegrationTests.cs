using System.Text;

using MaksIT.PodmanClientDotNet.Streaming;

namespace MaksIT.PodmanClientDotNet.Tests;

[Trait("Category", "Integration")]
public class PodmanClientStreamingIntegrationTests {
  private readonly IPodmanClient _client = PodmanClientTestFixture.CreateClient();

  [Fact]
  public async Task AttachContainerSessionAsync_ReadsStdoutFromRunningContainer() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var name = $"attach-session-{Guid.NewGuid():N}";
    const string image = "alpine:latest";

    await PullImageAsync(image);
    var containerId = await CreateContainerAsync(name, image, ["sh", "-c", "echo hello-attach"]);
    await StartContainerAsync(containerId);

    var attachResult = await _client.AttachContainerSessionAsync(
      containerId,
      stream: true,
      stdout: true,
      stderr: true,
      stdin: false,
      tty: false,
      cancellationToken: cancellationToken);

    PodmanClientTestFixture.AssertSuccess(attachResult);
    await using var session = attachResult.Value!;
    Assert.False(session.IsRawTerminal);

    var output = new StringBuilder();
    PodmanStreamFrame? frame;
    while ((frame = await session.ReadFrameAsync(cancellationToken)) is not null)
      output.Append(Encoding.UTF8.GetString(frame.Data));

    Assert.Contains("hello-attach", output.ToString());

    await StopContainerAsync(containerId);
    await ForceDeleteContainerAsync(containerId);
  }

  [Fact]
  public async Task StartExecSessionAsync_RunsCommandAndReadsOutput() {
    var cancellationToken = TestContext.Current.CancellationToken;
    var name = $"exec-session-{Guid.NewGuid():N}";
    const string image = "alpine:latest";

    await PullImageAsync(image);
    var containerId = await CreateContainerAsync(name, image, ["sh", "-c", "sleep 300"]);
    await StartContainerAsync(containerId);

    var createExec = await _client.CreateExecAsync(containerId, ["echo", "exec-ok"]);
    PodmanClientTestFixture.AssertSuccess(createExec);
    var execId = createExec.Value!.Id!;

    var sessionResult = await _client.StartExecSessionAsync(execId, tty: false, cancellationToken: cancellationToken);
    PodmanClientTestFixture.AssertSuccess(sessionResult);

    await using var session = sessionResult.Value!;
    var output = new StringBuilder();
    PodmanStreamFrame? frame;
    while ((frame = await session.ReadFrameAsync(cancellationToken)) is not null)
      output.Append(Encoding.UTF8.GetString(frame.Data));

    Assert.Contains("exec-ok", output.ToString());

    await StopContainerAsync(containerId);
    await ForceDeleteContainerAsync(containerId);
  }

  [Fact]
  public async Task PullImageWithProgressAsync_YieldsStatusLines() {
    const string image = "alpine:latest";

    var cancellationToken = TestContext.Current.CancellationToken;
    var result = await _client.PullImageWithProgressAsync(image, cancellationToken: cancellationToken);
    PodmanClientTestFixture.AssertSuccess(result);

    await using var session = result.Value!;
    var lines = new List<string>();
    await foreach (var item in session.ReadProgressAsync(cancellationToken)) {
      if (!string.IsNullOrEmpty(item.Status))
        lines.Add(item.Status);
      if (!string.IsNullOrEmpty(item.Error))
        break;
    }

    Assert.NotEmpty(lines);
  }

  [Fact]
  public async Task AttachContainerSessionAsync_InvalidContainer_Fails() {
    var result = await _client.AttachContainerSessionAsync(
      "nonexistent-container-id",
      cancellationToken: TestContext.Current.CancellationToken);
    PodmanClientTestFixture.AssertFailure(result);
  }

  private async Task PullImageAsync(string image) {
    var result = await _client.PullImageAsync(image);
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private async Task<string> CreateContainerAsync(string name, string image, List<string> command) {
    var result = await _client.CreateContainerAsync(name: name, image: image, command: command);
    string? containerId = null;
    PodmanClientTestFixture.AssertSuccess(result, value => containerId = value!.Id);
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
}
