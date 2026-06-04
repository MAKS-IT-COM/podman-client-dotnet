
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Startup Health Config).
/// </summary>

public class StartupHealthConfig {
  public long Interval { get; set; }
  public int Retries { get; set; }
  public long StartInterval { get; set; }
  public long StartPeriod { get; set; }
  public int Successes { get; set; }
  public List<string>? Test { get; set; }
  public long Timeout { get; set; }
}