
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Image Volume).
/// </summary>

public class ImageVolume {
  public string? Destination { get; set; }
  public bool ReadWrite { get; set; }
  public string? Source { get; set; }
  public string? SubPath { get; set; }
}