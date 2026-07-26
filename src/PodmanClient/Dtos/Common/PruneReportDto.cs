namespace MaksIT.PodmanClientDotNet.Dtos.Common;

/// <summary>Single entry from libpod prune endpoints that return a JSON array.</summary>
public sealed class PruneReportEntryDto {
  public string? Id { get; set; }
  public long Size { get; set; }
  public string? Err { get; set; }
}

/// <summary>Legacy aggregate prune fields (kept for compatibility where applicable).</summary>
public sealed class PruneReportDto {
  public string[]? Id { get; set; }
  public string[]? IdDeleted { get; set; }
  public ulong? Size { get; set; }
  public ulong? SpaceReclaimed { get; set; }
  public string[]? PodsDeleted { get; set; }
  public string[]? NetworksDeleted { get; set; }
  public string[]? VolumesDeleted { get; set; }
  public string[]? ImagesDeleted { get; set; }
  public string[]? ContainersDeleted { get; set; }
}

/// <summary>Response from <c>POST /libpod/system/prune</c>.</summary>
public sealed class SystemPruneReportDto {
  public List<PruneReportEntryDto>? PodPruneReport { get; set; }
  public List<PruneReportEntryDto>? ContainerPruneReports { get; set; }
  public List<PruneReportEntryDto>? ImagePruneReports { get; set; }
  public List<PruneReportEntryDto>? NetworkPruneReports { get; set; }
  public List<PruneReportEntryDto>? VolumePruneReports { get; set; }
  public long ReclaimedSpace { get; set; }
}
