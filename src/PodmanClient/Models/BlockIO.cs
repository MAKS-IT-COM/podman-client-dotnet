
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Block I O).
/// </summary>

public class BlockIO {
  public int LeafWeight { get; set; }
  public List<ThrottleDevice>? ThrottleReadBpsDevice { get; set; }
  public List<ThrottleDevice>? ThrottleReadIopsDevice { get; set; }
  public List<ThrottleDevice>? ThrottleWriteBpsDevice { get; set; }
  public List<ThrottleDevice>? ThrottleWriteIopsDevice { get; set; }
  public int Weight { get; set; }
  public List<WeightDevice>? WeightDevice { get; set; }
}