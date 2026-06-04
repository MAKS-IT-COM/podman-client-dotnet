
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Linux Device).
/// </summary>

public class LinuxDevice {
  public int FileMode { get; set; }
  public int Gid { get; set; }
  public int Major { get; set; }
  public int Minor { get; set; }
  public string? Path { get; set; }
  public string? Type { get; set; }
  public int Uid { get; set; }
}