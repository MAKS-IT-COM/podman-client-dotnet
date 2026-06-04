
namespace MaksIT.PodmanClientDotNet.Models.Exec;

/// <summary>
/// Libpod API request body for Start Exec request.
/// </summary>

public class StartExecRequest {
  public bool Detach { get; set; }
  public bool Tty { get; set; }
  public int? Height { get; set; } // Optional, nullable if not provided
  public int? Width { get; set; }  // Optional, nullable if not provided
}