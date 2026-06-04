namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image Import).
/// </summary>

public sealed class ImageImportDto {
  public string? Id { get; set; }
  public string[]? Repotags { get; set; }
}
