namespace MaksIT.PodmanClientDotNet.Dtos.Container;

/// <summary>
/// Result of mounting a container's root filesystem (libpod returns a plain path string).
/// </summary>
public sealed class ContainerMountDto {
  public string? Path { get; set; }
}
