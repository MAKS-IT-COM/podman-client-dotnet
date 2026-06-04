
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (I D Mapping).
/// </summary>

public class IDMapping {
  public int ContainerId { get; set; }
  public int HostId { get; set; }
  public int Size { get; set; }
}