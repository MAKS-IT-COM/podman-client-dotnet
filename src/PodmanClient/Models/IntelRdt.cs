
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Intel Rdt).
/// </summary>

public class IntelRdt {
  public string? ClosId { get; set; }
  public bool EnableCMT { get; set; }
  public bool EnableMBM { get; set; }
  public string? L3CacheSchema { get; set; }
  public string? MemBwSchema { get; set; }
}