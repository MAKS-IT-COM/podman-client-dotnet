namespace MaksIT.PodmanClientDotNet.Streaming;

/// <summary>
/// Multiplexed attach stream identifier (Docker/Podman raw stream protocol).
/// </summary>
public enum PodmanStreamType : byte {
  Stdin = 0,
  Stdout = 1,
  Stderr = 2,
  System = 3,
}
