
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Linux Resources).
/// </summary>

public class LinuxResources {
  public BlockIO? BlockIO { get; set; }
  public CPU? CPU { get; set; }
  public List<LinuxDeviceCgroup>? Devices { get; set; }
  public List<HugepageLimit>? HugepageLimits { get; set; }
  public Memory? Memory { get; set; }
  public LinuxNetwork? Network { get; set; }
  public Pids? Pids { get; set; }
  public Dictionary<string, RdmaResource>? Rdma { get; set; }
  public Dictionary<string, string>? Unified { get; set; }
}