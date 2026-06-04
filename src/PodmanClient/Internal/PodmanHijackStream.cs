using System.Net.Sockets;

namespace MaksIT.PodmanClientDotNet.Internal;

/// <summary>
/// Duplex network stream returned from a hijacked Podman HTTP connection.
/// </summary>
internal sealed class PodmanHijackStream : Stream {
  private readonly Stream _inner;
  private readonly TcpClient? _tcpClient;

  public PodmanHijackStream(Stream inner, TcpClient? tcpClient = null) {
    _inner = inner;
    _tcpClient = tcpClient;
  }

  public override bool CanRead => _inner.CanRead;
  public override bool CanSeek => false;
  public override bool CanWrite => _inner.CanWrite;
  public override long Length => throw new NotSupportedException();
  public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

  public void CloseWrite() {
    if (_tcpClient?.Client.Connected == true)
      _tcpClient.Client.Shutdown(SocketShutdown.Send);
  }

  public override void Flush() => _inner.Flush();
  public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
  public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
  public override void SetLength(long value) => throw new NotSupportedException();
  public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);

  public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
    _inner.ReadAsync(buffer, offset, count, cancellationToken);

  public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
    _inner.ReadAsync(buffer, cancellationToken);

  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
    _inner.WriteAsync(buffer, offset, count, cancellationToken);

  public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) =>
    _inner.WriteAsync(buffer, cancellationToken);

  protected override void Dispose(bool disposing) {
    if (disposing) {
      _inner.Dispose();
      _tcpClient?.Dispose();
    }
    base.Dispose(disposing);
  }
}
