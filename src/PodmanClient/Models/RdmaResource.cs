
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Rdma Resource).
/// </summary>

public class RdmaResource {
  public int HcaHandles { get; set; }
  public int HcaObjects { get; set; }
}