
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (I D Mapping Options).
/// </summary>

public class IDMappingOptions {
  public bool AutoUserNs { get; set; }
  public AutoUserNsOptions? AutoUserNsOpts { get; set; }
  public List<IDMapping>? GIDMap { get; set; }
  public bool HostGIDMapping { get; set; }
  public bool HostUIDMapping { get; set; }
  public List<IDMapping>? UIDMap { get; set; }
}