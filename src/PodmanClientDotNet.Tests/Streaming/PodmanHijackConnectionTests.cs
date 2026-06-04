using System.Buffers.Binary;
using System.Text;

using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.PodmanClientDotNet.Streaming;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

public class PodmanHijackConnectionTests {
  [Fact]
  public async Task ConnectAsync_ReadsMultiplexedPayloadFromMockServer() {
    await using var server = new PodmanHijackMockServer();
    using var payloadStream = new MemoryStream();
    PodmanMultiplexedProtocol.WriteFrame(payloadStream, PodmanStreamType.Stdout, "ok"u8.ToArray());
    var payload = payloadStream.ToArray();

    var cancellationToken = TestContext.Current.CancellationToken;
    var acceptTask = server.AcceptAndSendHijackResponseAsync(payload, cancellationToken);

    await using var hijack = await PodmanHijackConnection.ConnectAsync(
      new Uri($"http://127.0.0.1:{server.Port}"),
      "v1.41",
      HttpMethod.Post,
      "/libpod/containers/test/attach",
      "?stdin=1&stdout=1&stderr=1&stream=1",
      requestBody: null,
      cancellationToken
    );

    await acceptTask;

    await using var session = new PodmanAttachSession(hijack, isRawTerminal: false);
    var frame = await session.ReadFrameAsync(cancellationToken);
    Assert.NotNull(frame);
    Assert.Equal(PodmanStreamType.Stdout, frame!.StreamType);
    Assert.Equal("ok", Encoding.UTF8.GetString(frame.Data));
  }

  [Fact]
  public void WriteFrame_StdinType_MatchesProtocol() {
    using var stream = new MemoryStream();
    PodmanMultiplexedProtocol.WriteFrame(stream, PodmanStreamType.Stdin, [0x41]);
    stream.Position = 0;
    var header = new byte[8];
    stream.ReadExactly(header);
    Assert.Equal(0, header[0]);
    Assert.Equal(1u, BinaryPrimitives.ReadUInt32BigEndian(header.AsSpan(4)));
  }
}
