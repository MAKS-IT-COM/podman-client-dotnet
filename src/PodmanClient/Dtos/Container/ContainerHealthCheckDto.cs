namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Container Health Check).
/// </summary>

public sealed class ContainerHealthCheckDto {
  public string? Status { get; set; }
  public int FailingStreak { get; set; }
  public ContainerHealthLogDto[]? Log { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container Health Log).
/// </summary>

public sealed class ContainerHealthLogDto {
  public string? Start { get; set; }
  public string? End { get; set; }
  public int ExitCode { get; set; }
  public string? Output { get; set; }
}
