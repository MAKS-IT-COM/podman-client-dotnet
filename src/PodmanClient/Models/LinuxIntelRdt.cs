
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Linux Intel Rdt).
/// </summary>

public class LinuxIntelRdt {
  public string? ClosID { get; set; }
  public bool EnableCMT { get; set; }
  public bool EnableMBM { get; set; }
  public string? L3CacheSchema { get; set; }
  public string? MemBwSchema { get; set; }
}