using MaksIT.PodmanClientDotNet.Streaming;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

public class PodmanAttachSessionCloseWriteTests {
  [Fact]
  public async Task CloseWriteAsync_DoesNotThrowForNonHijackStream() {
    using var stream = new MemoryStream();
    await using var session = new PodmanAttachSession(stream, isRawTerminal: false, ownsStream: false);
    await session.CloseWriteAsync(TestContext.Current.CancellationToken);
    await session.WriteStdinAsync(ReadOnlyMemory<byte>.Empty, TestContext.Current.CancellationToken);
  }
}
