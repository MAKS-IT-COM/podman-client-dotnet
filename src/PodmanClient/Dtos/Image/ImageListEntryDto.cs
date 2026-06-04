namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image List Entry).
/// </summary>

public sealed class ImageListEntryDto {
  public string? Id { get; set; }
  public string[]? Names { get; set; }
  public string? Digest { get; set; }
  public long Created { get; set; }
  public long Size { get; set; }
  public long SharedSize { get; set; }
  public string? ParentId { get; set; }
  public string? RepoTags { get; set; }
  public string? RepoDigests { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Image Search Result).
/// </summary>

public sealed class ImageSearchResultDto {
  public string? Name { get; set; }
  public string? Description { get; set; }
  public int Stars { get; set; }
  public bool IsOfficial { get; set; }
  public bool IsAutomated { get; set; }
}
