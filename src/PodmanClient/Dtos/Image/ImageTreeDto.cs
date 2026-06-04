namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image Tree).
/// </summary>

public sealed class ImageTreeDto {
  public string? Id { get; set; }
  public ImageTreeLayerDto[]? Layers { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Image Tree Layer).
/// </summary>

public sealed class ImageTreeLayerDto {
  public string? Id { get; set; }
  public string? Parent { get; set; }
  public string[]? Tags { get; set; }
}
