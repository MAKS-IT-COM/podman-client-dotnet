namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Create Container response).
/// </summary>

public sealed class CreateContainerResponseDto {
  public string? Id { get; set; }
  public string[]? Warnings { get; set; }
}
