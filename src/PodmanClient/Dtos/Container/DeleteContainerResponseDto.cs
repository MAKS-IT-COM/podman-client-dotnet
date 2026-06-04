namespace MaksIT.PodmanClientDotNet.Dtos.Container;
/// <summary>
/// Deserialized Podman libpod API payload (Delete Container response).
/// </summary>

public sealed class DeleteContainerResponseDto {
  public string? Err { get; set; }
  public string? Id { get; set; }
}
