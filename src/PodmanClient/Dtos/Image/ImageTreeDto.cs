namespace MaksIT.PodmanClientDotNet.Dtos.Image;

/// <summary>
/// Deserialized Podman libpod API payload (Image Tree).
/// </summary>
public sealed class ImageTreeDto {
  public string? Tree { get; set; }
}
