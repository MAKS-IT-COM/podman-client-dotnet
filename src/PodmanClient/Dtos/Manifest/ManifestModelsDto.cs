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
  public int? SchemaVersion { get; set; }
  public string? MediaType { get; set; }
  public string? Name { get; set; }
  public ManifestListSpecDto[]? Manifests { get; set; }
}

/// <summary>
/// Deserialized Podman libpod API payload (Manifest List Spec entry).
/// </summary>
public sealed class ManifestListSpecDto {
  public string? Digest { get; set; }
  public string? MediaType { get; set; }
  public long? Size { get; set; }
  public string? Image { get; set; }
  public ManifestPlatformDto? Platform { get; set; }
  public string? Os { get; set; }
  public string? Arch { get; set; }
}

/// <summary>
/// Platform object nested in a manifest list entry.
/// </summary>
public sealed class ManifestPlatformDto {
  public string? Architecture { get; set; }
  public string? Os { get; set; }
  public string? Variant { get; set; }
}

/// <summary>
/// Libpod ManifestModifyOptions body (v4+ PUT /libpod/manifests/{name}).
/// </summary>
public sealed class ManifestAddRequestDto {
  public List<string>? Images { get; set; }
  public string? Image { get; set; }
  public bool All { get; set; }
  public string? Operation { get; set; }
}
