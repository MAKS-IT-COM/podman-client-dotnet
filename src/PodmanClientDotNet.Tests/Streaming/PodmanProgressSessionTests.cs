using System.Text;

using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.PodmanClientDotNet.Streaming;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

public class PodmanProgressSessionTests {
  [Fact]
  public async Task ReadProgressAsync_ParsesNdjsonLines() {
    var json = "{\"status\":\"Pulling fs layer\"}\n{\"id\":\"abc\"}\n";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
    await using var session = new PodmanProgressSession<PullImageResponseDto>(stream, ownsStream: false);

    var items = new List<PullImageResponseDto>();
    await foreach (var item in session.ReadProgressAsync(TestContext.Current.CancellationToken))
      items.Add(item);

    Assert.Equal(2, items.Count);
    Assert.Equal("Pulling fs layer", items[0].Status);
    Assert.Equal("abc", items[1].Id);
  }
}
