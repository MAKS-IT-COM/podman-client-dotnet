
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (P O S I X Rlimit).
/// </summary>

public class POSIXRlimit {
  public long Hard { get; set; }
  public long Soft { get; set; }
  public string? Type { get; set; }
}