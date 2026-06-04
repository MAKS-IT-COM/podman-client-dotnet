namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Mounted Container).
/// </summary>

public sealed class MountedContainerDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Mountpoint { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Mounted Containers response).
/// </summary>

public sealed class MountedContainersResponseDto {
  public List<MountedContainerDto>? Containers { get; set; }
}
