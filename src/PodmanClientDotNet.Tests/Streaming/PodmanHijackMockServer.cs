using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MaksIT.PodmanClientDotNet.Tests.Streaming;

internal sealed class PodmanHijackMockServer : IAsyncDisposable {
  private readonly TcpListener _listener;
  private TcpClient? _client;

  public int Port { get; }

  public PodmanHijackMockServer() {
    _listener = new TcpListener(IPAddress.Loopback, 0);
    _listener.Start();
    Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
  }

  public async Task AcceptAndSendHijackResponseAsync(byte[] multiplexedPayload, CancellationToken cancellationToken = default) {
    _client = await _listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
    var stream = _client.GetStream();
    using var reader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);

    _ = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
    while (true) {
      var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
      if (string.IsNullOrEmpty(line))
        break;
    }

    var response =
      "HTTP/1.1 200 OK\r\nContent-Type: application/vnd.docker.raw-stream\r\nConnection: Upgrade\r\nUpgrade: tcp\r\n\r\n";
    var responseBytes = Encoding.ASCII.GetBytes(response);
    await stream.WriteAsync(responseBytes, cancellationToken).ConfigureAwait(false);
    if (multiplexedPayload.Length > 0)
      await stream.WriteAsync(multiplexedPayload, cancellationToken).ConfigureAwait(false);
  }

  public async ValueTask DisposeAsync() {
    _client?.Dispose();
    _listener.Stop();
    await Task.CompletedTask.ConfigureAwait(false);
  }
}
