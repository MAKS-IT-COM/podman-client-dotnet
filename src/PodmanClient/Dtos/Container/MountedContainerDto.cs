namespace MaksIT.PodmanClientDotNet.Dtos.Container;

/// <summary>
/// Libpod <c>showmounted</c> returns a JSON array of single-entry maps (container id → mount path).
/// </summary>
public sealed class MountedContainersResponseDto : List<Dictionary<string, string>> {
}
