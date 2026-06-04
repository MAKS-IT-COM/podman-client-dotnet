namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Image Inspect).
/// </summary>

public sealed class ImageInspectDto {
  public string? Id { get; set; }
  public string[]? RepoTags { get; set; }
  public string[]? RepoDigests { get; set; }
  public long Size { get; set; }
  public string? Digest { get; set; }
  public string? Parent { get; set; }
  public string? Comment { get; set; }
  public string? Created { get; set; }
  public ImageConfigDto? Config { get; set; }
  public ImageRootFsDto? RootFS { get; set; }
  public string? Architecture { get; set; }
  public string? Os { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Image Config).
/// </summary>

public sealed class ImageConfigDto {
  public string? Hostname { get; set; }
  public string? User { get; set; }
  public string[]? Env { get; set; }
  public string[]? Cmd { get; set; }
  public string[]? Entrypoint { get; set; }
  public string? WorkingDir { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Image Root Fs).
/// </summary>

public sealed class ImageRootFsDto {
  public string? Type { get; set; }
  public string[]? Layers { get; set; }
}
