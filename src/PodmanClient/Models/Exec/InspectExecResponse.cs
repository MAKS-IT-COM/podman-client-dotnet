
namespace MaksIT.PodmanClientDotNet.Models.Exec;

/// <summary>
/// Libpod API response body for Inspect Exec process config.
/// </summary>

public class InspectExecProcess {
  public string[]? Arguments { get; set; }
  public string? Entrypoint { get; set; }
  public bool Privileged { get; set; }
  public bool Tty { get; set; }
  public string? User { get; set; }
}

/// <summary>
/// Libpod API response body for Inspect Exec response.
/// </summary>

public class InspectExecResponse {
  public bool CanRemove { get; set; }
  public string? ContainerID { get; set; }
  public string? DetachKeys { get; set; }
  public int ExitCode { get; set; }
  public string? ID { get; set; }
  public bool OpenStderr { get; set; }
  public bool OpenStdin { get; set; }
  public bool OpenStdout { get; set; }
  public bool Running { get; set; }
  public int Pid { get; set; }
  public InspectExecProcess? ProcessConfig { get; set; }
}
