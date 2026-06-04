using System.Buffers.Binary;
using System.Text;

using MaksIT.PodmanClientDotNet.Streaming;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

public class PodmanMultiplexedProtocolTests {
  [Fact]
  public void WriteFrame_WritesHeaderAndPayload() {
    using var stream = new MemoryStream();
    var payload = "hello"u8.ToArray();

    PodmanMultiplexedProtocol.WriteFrame(stream, PodmanStreamType.Stdout, payload);

    stream.Position = 0;
    var header = new byte[8];
    Assert.Equal(8, stream.Read(header));
    Assert.Equal((byte)PodmanStreamType.Stdout, header[0]);
    Assert.Equal(5u, BinaryPrimitives.ReadUInt32BigEndian(header.AsSpan(4)));

    var readPayload = new byte[5];
    Assert.Equal(5, stream.Read(readPayload));
    Assert.Equal("hello", Encoding.UTF8.GetString(readPayload));
  }

  [Fact]
  public async Task TryReadHeaderAsync_ReadsStdoutFrame() {
    using var stream = new MemoryStream();
    PodmanMultiplexedProtocol.WriteFrame(stream, PodmanStreamType.Stderr, "err"u8.ToArray());
    stream.Position = 0;

    var header = await PodmanMultiplexedProtocol.TryReadHeaderAsync(stream, new byte[8], CancellationToken.None);
    Assert.NotNull(header);
    Assert.Equal(PodmanStreamType.Stderr, header.Value.Type);
    Assert.Equal(3, header.Value.PayloadSize);
  }
}
