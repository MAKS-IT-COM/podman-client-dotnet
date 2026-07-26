namespace MaksIT.PodmanClientDotNet.Dtos.Image;

/// <summary>
/// Deserialized Podman libpod API payload (Image List Entry).
/// </summary>
public sealed class ImageListEntryDto {
  public string? Id { get; set; }
  public string? ParentId { get; set; }
  public string[]? RepoTags { get; set; }
  public string[]? RepoDigests { get; set; }
  public string[]? Names { get; set; }
  public string[]? History { get; set; }
  public string? Digest { get; set; }
  public long Created { get; set; }
  public long Size { get; set; }
  public long SharedSize { get; set; }
  public long VirtualSize { get; set; }
  public long Containers { get; set; }
  public string? Arch { get; set; }
  public string? Os { get; set; }
  public bool IsManifestList { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
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
