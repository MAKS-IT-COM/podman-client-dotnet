
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Network Priority).
/// </summary>

public class NetworkPriority {
  public string? Name { get; set; }
  public int Priority { get; set; }
}