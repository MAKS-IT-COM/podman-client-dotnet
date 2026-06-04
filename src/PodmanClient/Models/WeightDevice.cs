
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Weight Device).
/// </summary>

public class WeightDevice {
  public int LeafWeight { get; set; }
  public int Major { get; set; }
  public int Minor { get; set; }
  public int Weight { get; set; }
}