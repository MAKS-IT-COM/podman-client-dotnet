
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod container or image specification model (Namespace).
/// </summary>

public class Namespace {
  public string? Nsmode { get; set; }
  public string? Value { get; set; }
}