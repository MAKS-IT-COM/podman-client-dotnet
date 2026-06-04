
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Mount).
/// </summary>

public class Mount {
  public BindOptions? BindOptions { get; set; }
  public string? Consistency { get; set; }
  public bool ReadOnly { get; set; }
  public string? Source { get; set; }
  public string? Target { get; set; }
  public TmpfsOptions? TmpfsOptions { get; set; }
  public string? Type { get; set; }
  public VolumeOptions? VolumeOptions { get; set; }
}