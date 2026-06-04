
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Secret Prop).
/// </summary>

public class SecretProp {
  public string? Key { get; set; }
  public string? Secret { get; set; }
}