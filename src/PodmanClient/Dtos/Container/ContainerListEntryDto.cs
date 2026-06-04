namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Container List Entry).
/// </summary>

public sealed class ContainerListEntryDto {
  public string? Id { get; set; }
  public string[]? Names { get; set; }
  public string? Image { get; set; }
  public string? ImageID { get; set; }
  public string? Command { get; set; }
  public long Created { get; set; }
  public string? State { get; set; }
  public string? Status { get; set; }
  public string? Pod { get; set; }
  public string? PodName { get; set; }
  public bool AutoRemove { get; set; }
}
