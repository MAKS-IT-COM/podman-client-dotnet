
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Driver Config).
/// </summary>

public class DriverConfig {
  public string? Name { get; set; }
  public Dictionary<string, string>? Options { get; set; }
}