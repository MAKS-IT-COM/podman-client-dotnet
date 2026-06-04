using System.Text;

using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Internal;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

public class PodmanNdjsonStreamsTests {
  private static readonly ILogger Logger = NullLogger.Instance;

  [Fact]
  public async Task DrainPullOrPushAsync_SucceedsWhenNoErrorLines() {
    var json = "{\"status\":\"Pulling fs layer\"}\n{\"status\":\"Download complete\"}\n";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

    var result = await PodmanNdjsonStreams.DrainPullOrPushAsync(stream, Logger, "Pull image", TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);
  }

  [Fact]
  public async Task DrainPullOrPushAsync_FailsOnErrorLine() {
    var json = "{\"status\":\"Pulling\"}\n{\"error\":\"denied: access forbidden\"}\n";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

    var result = await PodmanNdjsonStreams.DrainPullOrPushAsync(stream, Logger, "Pull image", TestContext.Current.CancellationToken);

    Assert.False(result.IsSuccess);
    Assert.Contains("denied", result.Messages[0], StringComparison.OrdinalIgnoreCase);
  }

  [Fact]
  public async Task DrainBuildAsync_ReturnsIdFromProgressLines() {
    var json = "{\"stream\":\"Step 1/1\"}\n{\"id\":\"sha256:abc123\"}\n";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

    var result = await PodmanNdjsonStreams.DrainBuildAsync(stream, Logger, TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);
    Assert.Equal("sha256:abc123", result.Value!.Id);
  }

  [Fact]
  public async Task DrainBuildAsync_FailsOnErrorField() {
    var json = "{\"stream\":\"Step 1/1\"}\n{\"error\":\"Dockerfile parse error\"}\n";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

    var result = await PodmanNdjsonStreams.DrainBuildAsync(stream, Logger, TestContext.Current.CancellationToken);

    Assert.False(result.IsSuccess);
    Assert.Contains("Dockerfile", result.Messages[0], StringComparison.OrdinalIgnoreCase);
  }
}
