namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image Load).
/// </summary>

public sealed class ImageLoadDto {
  public string[]? Names { get; set; }
  public string[]? LoadedImages { get; set; }
}
