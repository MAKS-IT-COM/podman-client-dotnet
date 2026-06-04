
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Named Volume).
/// </summary>

public class NamedVolume {
  public string? Dest { get; set; }
  public bool IsAnonymous { get; set; }
  public string? Name { get; set; }
  public List<string>? Options { get; set; }
  public string? SubPath { get; set; }
}