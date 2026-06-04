namespace MaksIT.PodmanClientDotNet.Dtos.Container;

/// <summary>Podman returns a JSON array of filesystem change paths.</summary>
public sealed class ContainerChangesDto : List<string> {
}
