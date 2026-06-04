namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Container Stats).
/// </summary>

public sealed class ContainerStatsDto {
  public string? Name { get; set; }
  public string? Id { get; set; }
  public ContainerStatsCpuDto? CpuStats { get; set; }
  public ContainerStatsMemoryDto? MemoryStats { get; set; }
  public ContainerStatsNetworkDto[]? Networks { get; set; }
  public string? Read { get; set; }
  public string? Preread { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container Stats Cpu).
/// </summary>

public sealed class ContainerStatsCpuDto {
  public ulong TotalUsage { get; set; }
  public ulong SystemUsage { get; set; }
  public ulong KernelMode { get; set; }
  public ulong UserMode { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container Stats Memory).
/// </summary>

public sealed class ContainerStatsMemoryDto {
  public ulong Usage { get; set; }
  public ulong MaxUsage { get; set; }
  public ulong Limit { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container Stats Network).
/// </summary>

public sealed class ContainerStatsNetworkDto {
  public ulong RxBytes { get; set; }
  public ulong TxBytes { get; set; }
}
