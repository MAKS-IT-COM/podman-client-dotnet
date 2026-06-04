using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace MaksIT.PodmanClientDotNet.Internal;

internal static class PodmanHijackConnection {
  public static async Task<PodmanHijackStream> ConnectAsync(
    Uri baseAddress,
    string apiVersion,
    HttpMethod method,
    string libpodPath,
    string? queryString,
    byte[]? requestBody,
    CancellationToken cancellationToken
  ) {
    ArgumentNullException.ThrowIfNull(baseAddress);
    var host = baseAddress.Host;
    var port = baseAddress.IsDefaultPort
      ? baseAddress.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ? 443 : 80
      : baseAddress.Port;

    var tcpClient = new TcpClient();
    await tcpClient.ConnectAsync(host, port, cancellationToken).ConfigureAwait(false);

    Stream stream = tcpClient.GetStream();
    if (baseAddress.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase)) {
      var ssl = new SslStream(stream, leaveInnerStreamOpen: false);
      await ssl.AuthenticateAsClientAsync(new SslClientAuthenticationOptions { TargetHost = host }, cancellationToken)
        .ConfigureAwait(false);
      stream = ssl;
    }

    var path = $"/{apiVersion.Trim('/')}{libpodPath}{queryString}";
    var request = BuildRequest(method, path, host, requestBody);
    await stream.WriteAsync(request, cancellationToken).ConfigureAwait(false);

    await ReadResponseHeadersAsync(stream, cancellationToken).ConfigureAwait(false);
    return new PodmanHijackStream(stream, tcpClient);
  }

  private static byte[] BuildRequest(HttpMethod method, string path, string host, byte[]? body) {
    var sb = new StringBuilder();
    sb.Append(method.Method).Append(' ').Append(path).Append(" HTTP/1.1\r\n");
    sb.Append("Host: ").Append(host).Append("\r\n");
    sb.Append("Connection: Upgrade\r\n");
    sb.Append("Upgrade: tcp\r\n");
    sb.Append("Accept: application/vnd.docker.raw-stream\r\n");
    if (body is { Length: > 0 }) {
      sb.Append("Content-Type: application/json\r\n");
      sb.Append("Content-Length: ").Append(body.Length).Append("\r\n");
    }
    sb.Append("\r\n");
    var headerBytes = Encoding.ASCII.GetBytes(sb.ToString());
    if (body is null or { Length: 0 })
      return headerBytes;

    var combined = new byte[headerBytes.Length + body.Length];
    headerBytes.CopyTo(combined, 0);
    body.CopyTo(combined, headerBytes.Length);
    return combined;
  }

  private static async Task ReadResponseHeadersAsync(Stream stream, CancellationToken cancellationToken) {
    var statusLine = await ReadAsciiLineAsync(stream, cancellationToken).ConfigureAwait(false)
      ?? throw new IOException("Empty response from Podman hijack endpoint.");

    if (!statusLine.Contains(" 200 ", StringComparison.Ordinal) && !statusLine.Contains(" 101 ", StringComparison.Ordinal))
      throw new IOException($"Podman hijack failed: {statusLine}");

    while (true) {
      var line = await ReadAsciiLineAsync(stream, cancellationToken).ConfigureAwait(false);
      if (line is null || line.Length == 0)
        break;
    }
  }

  private static async Task<string?> ReadAsciiLineAsync(Stream stream, CancellationToken cancellationToken) {
    var buffer = new StringBuilder();
    while (true) {
      var single = new byte[1];
      var read = await stream.ReadAsync(single, cancellationToken).ConfigureAwait(false);
      if (read == 0)
        return buffer.Length == 0 ? null : buffer.ToString();

      var b = single[0];
      if (b == '\n') {
        if (buffer.Length > 0 && buffer[^1] == '\r')
          buffer.Length--;
        return buffer.ToString();
      }

      buffer.Append((char)b);
    }
  }
}
