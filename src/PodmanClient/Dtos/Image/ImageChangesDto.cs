namespace MaksIT.PodmanClientDotNet.Dtos.Image;

/// <summary>
/// Single filesystem change entry from image/container changes.
/// </summary>
public sealed class ImageChangeEntryDto {
  public string? Path { get; set; }
  public int Kind { get; set; }
}

/// <summary>
/// Podman returns a JSON array of path/kind change objects.
/// </summary>
public sealed class ImageChangesDto : List<ImageChangeEntryDto> {
}
