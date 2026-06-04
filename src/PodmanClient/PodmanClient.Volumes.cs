using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Volume;
using MaksIT.PodmanClientDotNet.Models.Volume;
using MaksIT.Results;

public partial class PodmanClient {
  public Task<Result<VolumeInspectResponseDto?>> CreateVolumeAsync(
    CreateVolumeRequest request,
    CancellationToken cancellationToken = default
  ) =>
    PostJsonAsync<CreateVolumeRequest, VolumeInspectResponseDto>(
      "/libpod/volumes/create",
      "Create volume",
      request,
      cancellationToken: cancellationToken
    );

  public Task<Result<List<VolumeListEntryDto>?>> ListVolumesAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<List<VolumeListEntryDto>>("/libpod/volumes/json", "List volumes", cancellationToken: cancellationToken);

  public Task<Result<VolumeInspectResponseDto?>> InspectVolumeAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<VolumeInspectResponseDto>($"/libpod/volumes/{Uri.EscapeDataString(name)}/json", "Inspect volume", cancellationToken: cancellationToken);

  public Task<Result> DeleteVolumeAsync(string name, bool force = false, CancellationToken cancellationToken = default) =>
    DeleteWithoutBodyAsync(
      $"/libpod/volumes/{Uri.EscapeDataString(name)}",
      "Delete volume",
      [("force", force.ToString().ToLowerInvariant())],
      cancellationToken
    );

  public Task<Result<PruneReportDto?>> PruneVolumesAsync(CancellationToken cancellationToken = default) =>
    PostLibpodAsync<PruneReportDto>("/libpod/volumes/prune", "Prune volumes", cancellationToken: cancellationToken);
}
