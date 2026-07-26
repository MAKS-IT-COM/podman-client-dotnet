namespace MaksIT.PodmanClientDotNet.Dtos.System;

/// <summary>
/// Deserialized Podman libpod API payload (Libpod Version).
/// </summary>
public sealed class LibpodVersionDto {
  public LibpodVersionPlatformDto? Platform { get; set; }
  public List<LibpodVersionComponentDto>? Components { get; set; }
  public string? Version { get; set; }
  public string? ApiVersion { get; set; }
  public string? MinAPIVersion { get; set; }
  public string? GitCommit { get; set; }
  public string? GoVersion { get; set; }
  public string? Os { get; set; }
  public string? Arch { get; set; }
  public string? KernelVersion { get; set; }
  public string? BuildTime { get; set; }
}

/// <summary>
/// Platform object from libpod version.
/// </summary>
public sealed class LibpodVersionPlatformDto {
  public string? Name { get; set; }
}

/// <summary>
/// Component entry from libpod version.
/// </summary>
public sealed class LibpodVersionComponentDto {
  public string? Name { get; set; }
  public string? Version { get; set; }
  public Dictionary<string, string>? Details { get; set; }
}
