namespace MaksIT.PodmanClientDotNet.Dtos.System;

/// <summary>
/// Deserialized Podman libpod API payload (Info).
/// </summary>
public sealed class InfoDto {
  public InfoHostDto? Host { get; set; }
  public InfoStoreDto? Store { get; set; }
  public InfoVersionDto? Version { get; set; }
  public InfoPluginsDto? Plugins { get; set; }
}

/// <summary>
/// Version block under info.
/// </summary>
public sealed class InfoVersionDto {
  public string? APIVersion { get; set; }
  public string? Version { get; set; }
  public string? GoVersion { get; set; }
  public string? GitCommit { get; set; }
  public string? BuiltTime { get; set; }
  public long Built { get; set; }
  public string? BuildOrigin { get; set; }
  public string? OsArch { get; set; }
  public string? Os { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Info Host).
/// </summary>
public sealed class InfoHostDto {
  public string? Arch { get; set; }
  public string? BuildahVersion { get; set; }
  public string? CgroupManager { get; set; }
  public string? CgroupVersion { get; set; }
  public List<string>? CgroupControllers { get; set; }
  public InfoConmonDto? Conmon { get; set; }
  public int Cpus { get; set; }
  public InfoCpuUtilizationDto? CpuUtilization { get; set; }
  public string? DatabaseBackend { get; set; }
  public InfoDistributionDto? Distribution { get; set; }
  public string? EventLogger { get; set; }
  public long FreeLocks { get; set; }
  public string? Hostname { get; set; }
  public string? Kernel { get; set; }
  public string? LogDriver { get; set; }
  public long MemFree { get; set; }
  public long MemTotal { get; set; }
  public string? NetworkBackend { get; set; }
  public string? Os { get; set; }
  public string? OSType { get; set; }
}

/// <summary>
/// Conmon info under host.
/// </summary>
public sealed class InfoConmonDto {
  public string? Package { get; set; }
  public string? Path { get; set; }
  public string? Version { get; set; }
}

/// <summary>
/// CPU utilization percentages under host.
/// </summary>
public sealed class InfoCpuUtilizationDto {
  public double UserPercent { get; set; }
  public double SystemPercent { get; set; }
  public double IdlePercent { get; set; }
}

/// <summary>
/// Distribution object under host.
/// </summary>
public sealed class InfoDistributionDto {
  public string? Distribution { get; set; }
  public string? Version { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Info Store).
/// </summary>
public sealed class InfoStoreDto {
  public string? ConfigFile { get; set; }
  public InfoContainerStoreDto? ContainerStore { get; set; }
  public string? GraphRoot { get; set; }
  public string? GraphDriverName { get; set; }
  public Dictionary<string, string>? GraphOptions { get; set; }
  public long GraphRootAllocated { get; set; }
  public long GraphRootUsed { get; set; }
  public Dictionary<string, string>? GraphStatus { get; set; }
  public string? ImageCopyTmpDir { get; set; }
  public InfoImageStoreDto? ImageStore { get; set; }
  public string? RunRoot { get; set; }
  public bool TransientStore { get; set; }
  public string? VolumePath { get; set; }
}

/// <summary>
/// Container store counters under store.
/// </summary>
public sealed class InfoContainerStoreDto {
  public long Number { get; set; }
  public long Paused { get; set; }
  public long Running { get; set; }
  public long Stopped { get; set; }
}

/// <summary>
/// Image store counters under store.
/// </summary>
public sealed class InfoImageStoreDto {
  public long Number { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Info Plugins).
/// </summary>
public sealed class InfoPluginsDto {
  public string[]? Volume { get; set; }
  public string[]? Network { get; set; }
  public string[]? Log { get; set; }
  public string[]? Authorization { get; set; }
}
