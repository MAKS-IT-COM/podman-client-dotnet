namespace MaksIT.PodmanClientDotNet.Dtos.Pod;
/// <summary>
/// Deserialized Podman libpod API payload (Pod List Entry).
/// </summary>

public sealed class PodListEntryDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Status { get; set; }
  public string? CgroupParent { get; set; }
  public DateTime Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Namespace { get; set; }
  public string? RestartPolicy { get; set; }
  public ulong? StopTimeout { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Pod Kill Report).
/// </summary>

public sealed class PodKillReportDto {
  public string[]? Ids { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Pod Inspect).
/// </summary>

public sealed class PodInspectDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Status { get; set; }
  public string? CgroupParent { get; set; }
  public DateTime Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Namespace { get; set; }
  public string? RestartPolicy { get; set; }
  public ulong? StopTimeout { get; set; }
  public string[]? Containers { get; set; }
  public string? InfraContainerId { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Pod Top).
/// </summary>

public sealed class PodTopDto {
  public string[]? Titles { get; set; }
  public List<string[]>? Processes { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Pod Stats).
/// </summary>

public sealed class PodStatsDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? CPU { get; set; }
  public string? MemUsage { get; set; }
  public string? MemLimit { get; set; }
  public string? MemPercent { get; set; }
  public string? NetIO { get; set; }
  public string? BlockIO { get; set; }
  public string? PIDs { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Pod Stats response).
/// </summary>

public sealed class PodStatsResponseDto {
  public Dictionary<string, PodStatsDto>? Stats { get; set; }
}
