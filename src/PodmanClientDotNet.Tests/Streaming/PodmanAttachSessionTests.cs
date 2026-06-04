using System.Text;

using MaksIT.PodmanClientDotNet.Streaming;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

public class PodmanAttachSessionTests {
  [Fact]
  public async Task ReadFrameAsync_ReadsMultiplexedStdout() {
    using var stream = new MemoryStream();
    PodmanMultiplexedProtocol.WriteFrame(stream, PodmanStreamType.Stdout, "hello"u8.ToArray());
    stream.Position = 0;

    await using var session = new PodmanAttachSession(stream, isRawTerminal: false, ownsStream: false);
    var frame = await session.ReadFrameAsync(TestContext.Current.CancellationToken);

    Assert.NotNull(frame);
    Assert.Equal(PodmanStreamType.Stdout, frame!.StreamType);
    Assert.Equal("hello", Encoding.UTF8.GetString(frame.Data));
  }

  [Fact]
  public async Task WriteStdinAsync_WritesMultiplexedFrame() {
    using var stream = new MemoryStream();
    await using var session = new PodmanAttachSession(stream, isRawTerminal: false, ownsStream: false);

    await session.WriteStdinAsync("abc"u8.ToArray(), TestContext.Current.CancellationToken);

    stream.Position = 0;
    var header = new byte[8];
    stream.ReadExactly(header);
    Assert.Equal((byte)PodmanStreamType.Stdin, header[0]);
    Assert.Equal(3u, System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(header.AsSpan(4)));
    var payload = new byte[3];
    stream.ReadExactly(payload);
    Assert.Equal("abc", Encoding.UTF8.GetString(payload));
  }

  [Fact]
  public async Task ReadFrameAsync_RawTerminal_ReturnsChunk() {
    var payload = Encoding.UTF8.GetBytes("tty-data");
    using var stream = new MemoryStream(payload);
    await using var session = new PodmanAttachSession(stream, isRawTerminal: true, ownsStream: false);

    var cancellationToken = TestContext.Current.CancellationToken;
    var frame = await session.ReadFrameAsync(cancellationToken);
    Assert.NotNull(frame);
    Assert.Equal("tty-data", Encoding.UTF8.GetString(frame!.Data));
    Assert.Null(await session.ReadFrameAsync(cancellationToken));
  }
}
