namespace MaksIT.PodmanClientDotNet.Dtos.System;
/// <summary>
/// Deserialized Podman libpod API payload (Info).
/// </summary>

public sealed class InfoDto {
  public InfoHostDto? Host { get; set; }
  public InfoStoreDto? Store { get; set; }
  public Dictionary<string, object>? Version { get; set; }
  public InfoPluginsDto? Plugins { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Info Host).
/// </summary>

public sealed class InfoHostDto {
  public string? Arch { get; set; }
  public string? BuildahVersion { get; set; }
  public long Containers { get; set; }
  public string? Distribution { get; set; }
  public string? Kernel { get; set; }
  public string? MemTotal { get; set; }
  public int MemFree { get; set; }
  public string? OSType { get; set; }
  public string? OS { get; set; }
  public int CPUs { get; set; }
  public string? PodmanVersion { get; set; }
  public string? Machine { get; set; }
  public string? Hostname { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Info Store).
/// </summary>

public sealed class InfoStoreDto {
  public string? GraphRoot { get; set; }
  public string? GraphDriverName { get; set; }
  public Dictionary<string, string>? GraphOptions { get; set; }
  public long ImageStoreNumber { get; set; }
  public long RunRoot { get; set; }
  public long VolumePath { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Info Plugins).
/// </summary>

public sealed class InfoPluginsDto {
  public string[]? Volume { get; set; }
  public string[]? Network { get; set; }
  public string[]? Log { get; set; }
  public string[]? Authorization { get; set; }
}
