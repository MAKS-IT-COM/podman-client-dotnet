namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Container Wait).
/// </summary>

public sealed class ContainerWaitDto {
  public long StatusCode { get; set; }
  public ContainerWaitErrorDto? Error { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Container Wait Error).
/// </summary>

public sealed class ContainerWaitErrorDto {
  public string? Message { get; set; }
}
