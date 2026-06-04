using MaksIT.PodmanClientDotNet.Internal;

namespace MaksIT.PodmanClientDotNet.Streaming;

internal sealed class PodmanAttachSession : IPodmanAttachSession {
  private readonly Stream _stream;
  private readonly bool _ownsStream;
  private readonly byte[] _headerBuffer = new byte[PodmanMultiplexedProtocol.HeaderSize];
  private readonly byte[] _readBuffer = new byte[81920];
  private bool _writeClosed;

  public bool IsRawTerminal { get; }

  internal PodmanAttachSession(Stream stream, bool isRawTerminal, bool ownsStream = true) {
    _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    IsRawTerminal = isRawTerminal;
    _ownsStream = ownsStream;
  }

  public async Task<PodmanStreamFrame?> ReadFrameAsync(CancellationToken cancellationToken = default) {
    if (IsRawTerminal)
      return await ReadRawAsync(cancellationToken).ConfigureAwait(false);

    var header = await PodmanMultiplexedProtocol.TryReadHeaderAsync(_stream, _headerBuffer, cancellationToken)
      .ConfigureAwait(false);
    if (header is null)
      return null;

    var (type, size) = header.Value;
    if (size == 0)
      return new PodmanStreamFrame { StreamType = type, Data = [], IsEndOfStream = false };

    var payload = new byte[size];
    var read = await PodmanMultiplexedProtocol.ReadExactAsync(_stream, payload, cancellationToken)
      .ConfigureAwait(false);
    if (read < size)
      throw new EndOfStreamException("Unexpected end of multiplexed stream while reading payload.");

    return new PodmanStreamFrame {
      StreamType = type,
      Data = payload,
      IsEndOfStream = false,
    };
  }

  private async Task<PodmanStreamFrame?> ReadRawAsync(CancellationToken cancellationToken) {
    var read = await _stream.ReadAsync(_readBuffer, cancellationToken).ConfigureAwait(false);
    if (read == 0)
      return null;

    return new PodmanStreamFrame {
      StreamType = PodmanStreamType.Stdout,
      Data = _readBuffer.AsSpan(0, read).ToArray(),
      IsEndOfStream = false,
    };
  }

  public ValueTask WriteStdinAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default) {
    if (_writeClosed || data.IsEmpty)
      return ValueTask.CompletedTask;

    if (IsRawTerminal)
      return new ValueTask(WriteRawAsync(data, cancellationToken));

    PodmanMultiplexedProtocol.WriteFrame(_stream, PodmanStreamType.Stdin, data.Span);
    return ValueTask.CompletedTask;
  }

  private Task WriteRawAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken) =>
    _stream.WriteAsync(data, cancellationToken).AsTask();

  public ValueTask CloseWriteAsync(CancellationToken cancellationToken = default) {
    _writeClosed = true;
    if (_stream is PodmanHijackStream hijack)
      hijack.CloseWrite();
    return ValueTask.CompletedTask;
  }

  public async ValueTask DisposeAsync() {
    if (_ownsStream)
      await _stream.DisposeAsync().ConfigureAwait(false);
  }
}
