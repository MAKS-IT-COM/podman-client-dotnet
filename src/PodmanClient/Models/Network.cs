
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Linux network resource limits (class ID and priorities) for container creation.
/// </summary>
public class LinuxNetwork {
  public int ClassID { get; set; }
  public List<NetworkPriority>? Priorities { get; set; }
}
