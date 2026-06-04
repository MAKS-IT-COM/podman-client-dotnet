namespace MaksIT.PodmanClientDotNet.Dtos.Volume;
/// <summary>
/// Deserialized Podman libpod API payload (Volume List Entry).
/// </summary>

public sealed class VolumeListEntryDto {
  public string? Name { get; set; }
  public string? Driver { get; set; }
  public string? Mountpoint { get; set; }
  public DateTime Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Scope { get; set; }
  public VolumeListOptionsDto? Options { get; set; }
  public VolumeUsageDataDto? UsageData { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Volume List Options).
/// </summary>

public sealed class VolumeListOptionsDto {
  public string? Device { get; set; }
  public string? Type { get; set; }
  public string? Label { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Volume Usage Data).
/// </summary>

public sealed class VolumeUsageDataDto {
  public long Size { get; set; }
  public long RefCount { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Volume Inspect response).
/// </summary>

public sealed class VolumeInspectResponseDto {
  public string? Name { get; set; }
  public string? Driver { get; set; }
  public string? Mountpoint { get; set; }
  public DateTime Created { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Scope { get; set; }
  public VolumeListOptionsDto? Options { get; set; }
  public VolumeUsageDataDto? UsageData { get; set; }
}
