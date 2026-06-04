
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Schema2 Health Config).
/// </summary>

public class Schema2HealthConfig {
  public long Interval { get; set; }
  public int Retries { get; set; }
  public long StartInterval { get; set; }
  public long StartPeriod { get; set; }
  public List<string>? Test { get; set; }
  public long Timeout { get; set; }
}