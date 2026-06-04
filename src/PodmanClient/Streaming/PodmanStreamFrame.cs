namespace MaksIT.PodmanClientDotNet.Streaming;

/// <summary>
/// A single frame from a multiplexed Podman attach/exec stream.
/// </summary>
public sealed class PodmanStreamFrame {
  public required PodmanStreamType StreamType { get; init; }
  public required byte[] Data { get; init; }
  public bool IsEndOfStream { get; init; }
}
