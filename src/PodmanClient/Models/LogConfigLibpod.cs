
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Log Config Libpod).
/// </summary>

public class LogConfigLibpod {
  public string? Driver { get; set; }
  public Dictionary<string, string>? Options { get; set; }
  public string? Path { get; set; }
  public long Size { get; set; }
}