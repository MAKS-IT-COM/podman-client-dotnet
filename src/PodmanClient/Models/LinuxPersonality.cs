
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Linux Personality).
/// </summary>

public class LinuxPersonality {
  public string? Domain { get; set; }
  public List<string>? Flags { get; set; }
}