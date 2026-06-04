
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Port Mapping).
/// </summary>

public class PortMapping {
  public int ContainerPort { get; set; }
  public string? HostIp { get; set; }
  public int HostPort { get; set; }
  public string? Protocol { get; set; }
  public int Range { get; set; }
}