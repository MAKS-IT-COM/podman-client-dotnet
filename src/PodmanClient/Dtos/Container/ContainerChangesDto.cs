namespace MaksIT.PodmanClientDotNet.Dtos.Container;

/// <summary>
/// Single filesystem change entry from container changes.
/// </summary>
public sealed class ContainerChangeEntryDto {
  public string? Path { get; set; }
  public int Kind { get; set; }
}

/// <summary>
/// Podman returns a JSON array of path/kind change objects.
/// </summary>
public sealed class ContainerChangesDto : List<ContainerChangeEntryDto> {
}
