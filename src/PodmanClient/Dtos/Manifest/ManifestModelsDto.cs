namespace MaksIT.PodmanClientDotNet.Dtos.Manifest;
/// <summary>
/// Deserialized Podman libpod API payload (Manifest Create).
/// </summary>

public sealed class ManifestCreateDto {
  public string? Id { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Manifest Inspect).
/// </summary>

public sealed class ManifestInspectDto {
  public string? Name { get; set; }
  public ManifestListSpecDto[]? Manifests { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Manifest List Spec).
/// </summary>

public sealed class ManifestListSpecDto {
  public string? Digest { get; set; }
  public string? Image { get; set; }
  public string? Platform { get; set; }
  public string? Os { get; set; }
  public string? Arch { get; set; }
}
/// <summary>
/// Deserialized Podman libpod API payload (Manifest Add request).
/// </summary>

public sealed class ManifestAddRequestDto {
  public string Image { get; set; } = "";
  public bool All { get; set; }
  public string? Operation { get; set; }
}
