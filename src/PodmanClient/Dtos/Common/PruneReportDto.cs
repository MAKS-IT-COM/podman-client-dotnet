namespace MaksIT.PodmanClientDotNet.Dtos.Common;

/// <summary>Prune operation report (containers, images, volumes, pods, networks).</summary>
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
