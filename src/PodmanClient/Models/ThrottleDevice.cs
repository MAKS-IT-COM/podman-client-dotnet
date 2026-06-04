
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Throttle Device).
/// </summary>

public class ThrottleDevice {
  public int Major { get; set; }
  public int Minor { get; set; }
  public long Rate { get; set; }
}