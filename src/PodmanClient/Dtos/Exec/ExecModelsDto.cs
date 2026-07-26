namespace MaksIT.PodmanClientDotNet.Dtos.Exec;
/// <summary>
/// Deserialized Podman libpod API payload (Create Exec response).
/// </summary>

public sealed class CreateExecResponseDto {
  public string? Id { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Inspect Exec process config).
/// </summary>

public sealed class InspectExecProcessDto {
  public string[]? Arguments { get; set; }
  public string? Entrypoint { get; set; }
  public bool Privileged { get; set; }
  public bool Tty { get; set; }
  public string? User { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Inspect Exec response).
/// </summary>

public sealed class InspectExecResponseDto {
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
  public InspectExecProcessDto? ProcessConfig { get; set; }
}
