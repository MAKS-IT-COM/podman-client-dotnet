
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Linux Device Cgroup).
/// </summary>

public class LinuxDeviceCgroup {
  public string? Access { get; set; }
  public bool Allow { get; set; }
  public int Major { get; set; }
  public int Minor { get; set; }
  public string? Type { get; set; }
}