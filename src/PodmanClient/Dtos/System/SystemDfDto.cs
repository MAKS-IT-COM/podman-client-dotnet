namespace MaksIT.PodmanClientDotNet.Dtos.System;
/// <summary>
/// Deserialized Podman libpod API payload (System Df).
/// </summary>

public sealed class SystemDfDto {
  public SystemDfEntryDto[]? Images { get; set; }
  public SystemDfEntryDto[]? Containers { get; set; }
  public SystemDfEntryDto[]? Volumes { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (System Df Entry).
/// </summary>

public sealed class SystemDfEntryDto {
  public long Size { get; set; }
  public long Reclaimable { get; set; }
}
