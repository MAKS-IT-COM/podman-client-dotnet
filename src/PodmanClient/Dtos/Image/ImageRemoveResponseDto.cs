namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image Remove response).
/// </summary>

public sealed class ImageRemoveResponseDto {
  public ImageDeleteDto[]? DeletedImages { get; set; }
}
