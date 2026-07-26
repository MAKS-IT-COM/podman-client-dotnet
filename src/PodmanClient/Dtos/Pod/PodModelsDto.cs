namespace MaksIT.PodmanClientDotNet.Dtos.Pod;

/// <summary>
/// Container summary nested in pod list/inspect responses.
/// </summary>
public sealed class PodContainerSummaryDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Names { get; set; }
  public string? State { get; set; }
  public string? Status { get; set; }
  public int? RestartCount { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Pod List Entry).
/// </summary>
public sealed class PodListEntryDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Status { get; set; }
  public string? Cgroup { get; set; }
  public string? CgroupParent { get; set; }
  public string? Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Namespace { get; set; }
  public string? RestartPolicy { get; set; }
  public ulong? StopTimeout { get; set; }
  public string? InfraId { get; set; }
  public string[]? Networks { get; set; }
  public List<PodContainerSummaryDto>? Containers { get; set; }
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
  public string? State { get; set; }
  public string? Status { get; set; }
  public string? CgroupParent { get; set; }
  public string? Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Namespace { get; set; }
  public string? RestartPolicy { get; set; }
  public ulong? StopTimeout { get; set; }
  public List<PodContainerSummaryDto>? Containers { get; set; }
  public string? InfraContainerID { get; set; }
  public int? NumContainers { get; set; }
  public string[]? SharedNamespaces { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Pod Top).
/// </summary>
public sealed class PodTopDto {
  public string[]? Titles { get; set; }
  public List<string[]>? Processes { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Pod Stats entry).
/// </summary>
public sealed class PodStatsDto {
  public string? Id { get; set; }
  public string? CID { get; set; }
  public string? Pod { get; set; }
  public string? Name { get; set; }
  public string? CPU { get; set; }
  public string? MemUsage { get; set; }
  public string? MemUsageBytes { get; set; }
  public string? MemLimit { get; set; }
  public string? Mem { get; set; }
  public string? MemPercent { get; set; }
  public string? NetIO { get; set; }
  public string? BlockIO { get; set; }
  public string? PIDS { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Pod Stats response wrapper).
/// Prefer <see cref="List{PodStatsDto}"/> from the API; this type remains for callers that expect a named bag.
/// </summary>
public sealed class PodStatsResponseDto {
  public List<PodStatsDto>? Stats { get; set; }
}
