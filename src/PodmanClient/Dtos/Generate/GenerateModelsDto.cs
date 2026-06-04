namespace MaksIT.PodmanClientDotNet.Dtos.Generate;
/// <summary>
/// Deserialized Podman libpod API payload (Generate Systemd).
/// </summary>

public sealed class GenerateSystemdDto {
  public Dictionary<string, string>? Units { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Systemd Unit).
/// </summary>

public sealed class SystemdUnitDto {
  public string? Name { get; set; }
  public string? Content { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Play Kube Report).
/// </summary>

public sealed class PlayKubeReportDto {
  public string? Pod { get; set; }
  public string[]? Containers { get; set; }
  public string[]? Volumes { get; set; }
  public string[]? Secrets { get; set; }
  public string[]? Networks { get; set; }
}
