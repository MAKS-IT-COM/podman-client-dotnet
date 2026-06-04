namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Container Inspect).
/// </summary>

public sealed class ContainerInspectDto {
  public string? Id { get; set; }
  public string? Name { get; set; }
  public string? Image { get; set; }
  public string? ImageName { get; set; }
  public string? Pod { get; set; }
  public string? PodName { get; set; }
  public ContainerStateDto? State { get; set; }
  public ContainerConfigDto? Config { get; set; }
  public string? MountLabel { get; set; }
  public string? ProcessLabel { get; set; }
  public string? AppArmorProfile { get; set; }
  public string? HostnamePath { get; set; }
  public string? HostsPath { get; set; }
  public string? ResolvConfPath { get; set; }
  public string? Driver { get; set; }
  public string? OCIConfigPath { get; set; }
  public string? OCIRuntime { get; set; }
  public long Created { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container State).
/// </summary>

public sealed class ContainerStateDto {
  public string? Status { get; set; }
  public bool Running { get; set; }
  public bool Paused { get; set; }
  public bool Restarting { get; set; }
  public bool OOMKilled { get; set; }
  public bool Dead { get; set; }
  public int Pid { get; set; }
  public int ExitCode { get; set; }
  public string? Error { get; set; }
  public string? StartedAt { get; set; }
  public string? FinishedAt { get; set; }
  public string? Health { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container Config).
/// </summary>

public sealed class ContainerConfigDto {
  public string? Hostname { get; set; }
  public string? Domainname { get; set; }
  public string? User { get; set; }
  public bool AttachStdin { get; set; }
  public bool AttachStdout { get; set; }
  public bool AttachStderr { get; set; }
  public string? Tty { get; set; }
  public bool OpenStdin { get; set; }
  public bool StdinOnce { get; set; }
  public string[]? Env { get; set; }
  public string[]? Cmd { get; set; }
  public string? Image { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? WorkingDir { get; set; }
  public bool NetworkDisabled { get; set; }
  public string? StopSignal { get; set; }
}
