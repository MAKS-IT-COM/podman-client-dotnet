namespace MaksIT.PodmanClientDotNet.Dtos.Exec;
/// <summary>
/// Deserialized Podman libpod API payload (Create Exec response).
/// </summary>

public sealed class CreateExecResponseDto {
  public string? Id { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Inspect Exec response).
/// </summary>

public sealed class InspectExecResponseDto {
  public bool Running { get; set; }
  public int ExitCode { get; set; }
  public string? ProcessConfig { get; set; }
}
