using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Volume;
using MaksIT.PodmanClientDotNet.Models.Volume;
using MaksIT.Results;

/// <summary>
/// Volume create, list, inspect, delete, and prune endpoints.
/// </summary>
public interface IPodmanVolumesClient {
  Task<Result<VolumeInspectResponseDto?>> CreateVolumeAsync(CreateVolumeRequest request, CancellationToken cancellationToken = default);
  Task<Result<List<VolumeListEntryDto>?>> ListVolumesAsync(CancellationToken cancellationToken = default);
  Task<Result<VolumeInspectResponseDto?>> InspectVolumeAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> DeleteVolumeAsync(string name, bool force = false, CancellationToken cancellationToken = default);
  Task<Result<PruneReportDto?>> PruneVolumesAsync(CancellationToken cancellationToken = default);
}
