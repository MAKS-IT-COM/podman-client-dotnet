namespace MaksIT.PodmanClientDotNet.Dtos.Image;

/// <summary>
/// Deserialized Podman libpod API payload (Image Delete / Remove).
/// </summary>
public sealed class ImageDeleteDto {
  public string[]? Deleted { get; set; }
  public string[]? Untagged { get; set; }
  public int ExitCode { get; set; }
  public string[]? Errors { get; set; }
}
