namespace MaksIT.PodmanClientDotNet.Dtos.Build;
/// <summary>
/// Deserialized Podman libpod API payload (Build Report).
/// </summary>

public sealed class BuildReportDto {
  public string? Id { get; set; }
  public string[]? Names { get; set; }
}
