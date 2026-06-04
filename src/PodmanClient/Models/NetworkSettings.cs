
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Network Settings).
/// </summary>

public class NetworkSettings {
  public List<string>? Aliases { get; set; }
  public string? InterfaceName { get; set; }
  public List<string>? StaticIps { get; set; }
  public string? StaticMac { get; set; }
}