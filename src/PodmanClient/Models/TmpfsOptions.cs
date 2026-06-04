
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Tmpfs Options).
/// </summary>

public class TmpfsOptions {
  public int Mode { get; set; }
  public List<string>? Options { get; set; }
  public long SizeBytes { get; set; }
}