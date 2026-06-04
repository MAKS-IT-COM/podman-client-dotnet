
namespace MaksIT.PodmanClientDotNet.Models.Exec;

/// <summary>
/// Libpod API response body for Inspect Exec response.
/// </summary>

public class InspectExecResponse {
  public bool Running { get; set; }
  public int ExitCode { get; set; }
  public string? ProcessConfig { get; set; }
}