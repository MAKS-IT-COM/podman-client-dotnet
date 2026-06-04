
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Hugepage Limit).
/// </summary>

public class HugepageLimit {
  public long Limit { get; set; }
  public string? PageSize { get; set; }
}