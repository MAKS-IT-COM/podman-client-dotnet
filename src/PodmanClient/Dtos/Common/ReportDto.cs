namespace MaksIT.PodmanClientDotNet.Dtos.Common;
/// <summary>
/// Deserialized Podman libpod API payload (Report).
/// </summary>

public sealed class ReportDto {
  public string[]? Id { get; set; }
  public Dictionary<string, string>? Err { get; set; }
}
