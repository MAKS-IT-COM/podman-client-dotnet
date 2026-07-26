namespace MaksIT.PodmanClientDotNet.Dtos.Container;

/// <summary>
/// Deserialized Podman libpod API payload (Container Stats).
/// </summary>
public sealed class ContainerStatsDto {
  public string? Name { get; set; }
  public string? Id { get; set; }
  public string? Read { get; set; }
  public string? Preread { get; set; }
  public ContainerStatsCpuBlockDto? CpuStats { get; set; }
  public ContainerStatsCpuBlockDto? PrecpuStats { get; set; }
  public ContainerStatsMemoryDto? MemoryStats { get; set; }
  public Dictionary<string, ContainerStatsNetworkDto>? Networks { get; set; }
  public ContainerStatsPidsDto? PidsStats { get; set; }
  public int NumProcs { get; set; }
}

/// <summary>
/// CPU stats block from container stats.
/// </summary>
public sealed class ContainerStatsCpuBlockDto {
  public ContainerStatsCpuUsageDto? CpuUsage { get; set; }
  public ulong SystemCpuUsage { get; set; }
  public int OnlineCpus { get; set; }
  public double Cpu { get; set; }
}

/// <summary>
/// Nested CPU usage counters.
/// </summary>
public sealed class ContainerStatsCpuUsageDto {
  public ulong TotalUsage { get; set; }
  public ulong UsageInKernelmode { get; set; }
  public ulong UsageInUsermode { get; set; }
}

/// <summary>
/// Memory stats from container stats.
/// </summary>
public sealed class ContainerStatsMemoryDto {
  public ulong Usage { get; set; }
  public ulong MaxUsage { get; set; }
  public ulong Limit { get; set; }
}

/// <summary>
/// Per-interface network counters.
/// </summary>
public sealed class ContainerStatsNetworkDto {
  public ulong RxBytes { get; set; }
  public ulong RxPackets { get; set; }
  public ulong RxErrors { get; set; }
  public ulong RxDropped { get; set; }
  public ulong TxBytes { get; set; }
  public ulong TxPackets { get; set; }
  public ulong TxErrors { get; set; }
  public ulong TxDropped { get; set; }
}

/// <summary>
/// PID stats from container stats.
/// </summary>
public sealed class ContainerStatsPidsDto {
  public long Current { get; set; }
}

/// <summary>
/// Response from <c>GET /libpod/containers/stats</c> (multi-container).
/// </summary>
public sealed class ContainersStatsResponseDto {
  public string? Error { get; set; }
  public List<ContainerLibpodStatsDto>? Stats { get; set; }
}

/// <summary>
/// Libpod multi-container stats entry.
/// </summary>
public sealed class ContainerLibpodStatsDto {
  public double AvgCPU { get; set; }
  public string? ContainerID { get; set; }
  public string? Name { get; set; }
  public double CPU { get; set; }
  public long CPUNano { get; set; }
  public long CPUSystemNano { get; set; }
  public long SystemNano { get; set; }
  public long MemUsage { get; set; }
  public long MemLimit { get; set; }
  public double MemPerc { get; set; }
  public Dictionary<string, ContainerStatsNetworkDto>? Network { get; set; }
  public long BlockInput { get; set; }
  public long BlockOutput { get; set; }
  public long PIDs { get; set; }
  public long UpTime { get; set; }
  public long Duration { get; set; }
}
