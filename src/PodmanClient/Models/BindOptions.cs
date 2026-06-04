
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Bind Options).
/// </summary>

public class BindOptions {
  public bool CreateMountpoint { get; set; }
  public bool NonRecursive { get; set; }
  public string? Propagation { get; set; }
  public bool ReadOnlyForceRecursive { get; set; }
  public bool ReadOnlyNonRecursive { get; set; }
}