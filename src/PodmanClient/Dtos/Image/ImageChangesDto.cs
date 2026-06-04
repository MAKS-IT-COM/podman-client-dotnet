namespace MaksIT.PodmanClientDotNet.Dtos.Image;

/// <summary>Podman returns a JSON array of filesystem change paths.</summary>
public sealed class ImageChangesDto : List<string> {
}
