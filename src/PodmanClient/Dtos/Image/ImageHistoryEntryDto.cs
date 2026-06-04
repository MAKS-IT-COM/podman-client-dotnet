namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image History Entry).
/// </summary>

public sealed class ImageHistoryEntryDto {
  public string? Id { get; set; }
  public long Created { get; set; }
  public string? CreatedBy { get; set; }
  public string[]? Tags { get; set; }
  public long Size { get; set; }
  public string? Comment { get; set; }
}
