
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Memory).
/// </summary>

public class Memory {
  public bool CheckBeforeUpdate { get; set; }
  public bool DisableOOMKiller { get; set; }
  public long Kernel { get; set; }
  public long KernelTCP { get; set; }
  public long Limit { get; set; }
  public long Reservation { get; set; }
  public long Swap { get; set; }
  public int Swappiness { get; set; }
  public bool UseHierarchy { get; set; }
}