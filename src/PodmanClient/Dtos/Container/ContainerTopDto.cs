namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Container Top).
/// </summary>

public sealed class ContainerTopDto {
  public string[]? Titles { get; set; }
  public List<string[]>? Processes { get; set; }
}
