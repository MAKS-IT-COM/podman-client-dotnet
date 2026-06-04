using MaksIT.PodmanClientDotNet.Dtos.Manifest;
using MaksIT.Results;

/// <summary>
/// Manifest list create, inspect, add, push, and delete endpoints.
/// </summary>
public interface IPodmanManifestsClient {
  Task<Result<ManifestCreateDto?>> CreateManifestAsync(
    string name,
    string? image = null,
    bool all = false,
    CancellationToken cancellationToken = default
  );

  Task<Result> DeleteManifestAsync(string name, string? digest = null, CancellationToken cancellationToken = default);
  Task<Result<ManifestInspectDto?>> InspectManifestAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> AddToManifestAsync(string name, ManifestAddRequestDto request, CancellationToken cancellationToken = default);
  Task<Result> PushManifestAsync(string name, string destination, bool all = false, CancellationToken cancellationToken = default);
}
