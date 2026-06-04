namespace MaksIT.PodmanClientDotNet.Streaming;

/// <summary>
/// Full-duplex attach or exec session over a hijacked Podman HTTP connection.
/// </summary>
public interface IPodmanAttachSession : IAsyncDisposable {
  /// <summary>
  /// When true, the connection is a raw TTY stream without multiplex framing.
  /// </summary>
  bool IsRawTerminal { get; }

  /// <summary>
  /// Reads the next frame (multiplexed) or chunk (raw TTY).
  /// Returns null when the stream ends.
  /// </summary>
  Task<PodmanStreamFrame?> ReadFrameAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Writes bytes to container stdin (multiplexed framing applied when not raw TTY).
  /// </summary>
  ValueTask WriteStdinAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default);

  /// <summary>
  /// Signals end of stdin to the container process.
  /// </summary>
  ValueTask CloseWriteAsync(CancellationToken cancellationToken = default);
}
