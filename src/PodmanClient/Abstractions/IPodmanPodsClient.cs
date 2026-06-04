using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Pod;
using MaksIT.PodmanClientDotNet.Models.Pod;
using MaksIT.Results;

/// <summary>
/// Pod create, list, inspect, lifecycle, stats, and prune endpoints.
/// </summary>
public interface IPodmanPodsClient {
  Task<Result<PodListEntryDto?>> CreatePodAsync(PodCreateRequest request, CancellationToken cancellationToken = default);
  Task<Result<List<PodListEntryDto>?>> ListPodsAsync(bool all = false, CancellationToken cancellationToken = default);
  Task<Result<PodInspectDto?>> InspectPodAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> PodExistsAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> DeletePodAsync(string name, bool force = false, CancellationToken cancellationToken = default);
  Task<Result> StartPodAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> StopPodAsync(string name, int timeout = 10, CancellationToken cancellationToken = default);
  Task<Result> RestartPodAsync(string name, int timeout = 10, CancellationToken cancellationToken = default);
  Task<Result> KillPodAsync(string name, string? signal = null, CancellationToken cancellationToken = default);
  Task<Result> PausePodAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> UnpausePodAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<PruneReportDto?>> PrunePodsAsync(CancellationToken cancellationToken = default);
  Task<Result<PodTopDto?>> TopPodAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<PodStatsResponseDto?>> GetPodsStatsAsync(CancellationToken cancellationToken = default);
}
