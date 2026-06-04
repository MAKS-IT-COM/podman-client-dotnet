namespace MaksIT.PodmanClientDotNet.Dtos.System;
/// <summary>
/// Deserialized Podman libpod API payload (Libpod Ping).
/// </summary>

public sealed class LibpodPingDto {
  public bool Ping { get; set; }
}
