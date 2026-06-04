
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Volume Options).
/// </summary>

public class VolumeOptions {
  public DriverConfig? DriverConfig { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public bool NoCopy { get; set; }
  public string? Subpath { get; set; }
}