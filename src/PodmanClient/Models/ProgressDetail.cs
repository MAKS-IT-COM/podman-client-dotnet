
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Progress Detail).
/// </summary>

public class ProgressDetail {
  public long? Current { get; set; }
  public long? Total { get; set; }
}