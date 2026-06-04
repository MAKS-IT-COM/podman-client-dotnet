
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (C P U).
/// </summary>

public class CPU {
  public int Burst { get; set; }
  public string? Cpus { get; set; }
  public int Idle { get; set; }
  public string? Mems { get; set; }
  public int Period { get; set; }
  public int Quota { get; set; }
  public int RealtimePeriod { get; set; }
  public int RealtimeRuntime { get; set; }
  public int Shares { get; set; }
}