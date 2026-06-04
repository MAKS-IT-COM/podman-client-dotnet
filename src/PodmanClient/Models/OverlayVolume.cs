
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Overlay Volume).
/// </summary>

public class OverlayVolume {
  public string? Destination { get; set; }
  public List<string>? Options { get; set; }
  public string? Source { get; set; }
}