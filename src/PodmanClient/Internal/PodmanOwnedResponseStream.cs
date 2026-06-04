namespace MaksIT.PodmanClientDotNet.Internal;

/// <summary>
/// Wraps a response stream and disposes the underlying <see cref="HttpResponseMessage"/> when done.
/// </summary>
internal sealed class PodmanOwnedResponseStream : Stream {
  private readonly Stream _inner;
  private readonly HttpResponseMessage _response;
  private bool _disposed;

  public PodmanOwnedResponseStream(Stream inner, HttpResponseMessage response) {
    _inner = inner;
    _response = response;
  }

  public override bool CanRead => _inner.CanRead;
  public override bool CanSeek => _inner.CanSeek;
  public override bool CanWrite => _inner.CanWrite;
  public override long Length => _inner.Length;
  public override long Position { get => _inner.Position; set => _inner.Position = value; }

  public override void Flush() => _inner.Flush();
  public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
  public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
  public override void SetLength(long value) => _inner.SetLength(value);
  public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);

  public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
    _inner.ReadAsync(buffer, offset, count, cancellationToken);

  public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
    _inner.ReadAsync(buffer, cancellationToken);

  protected override void Dispose(bool disposing) {
    if (!_disposed && disposing) {
      _inner.Dispose();
      _response.Dispose();
      _disposed = true;
    }
    base.Dispose(disposing);
  }

  public override async ValueTask DisposeAsync() {
    if (!_disposed) {
      await _inner.DisposeAsync().ConfigureAwait(false);
      _response.Dispose();
      _disposed = true;
    }
    await base.DisposeAsync().ConfigureAwait(false);
  }
}
