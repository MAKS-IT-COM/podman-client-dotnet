using MaksIT.PodmanClientDotNet;
using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Pod;
using MaksIT.PodmanClientDotNet.Models.Pod;
using MaksIT.Results;

public partial class PodmanClient {
  public Task<Result<PodListEntryDto?>> CreatePodAsync(PodCreateRequest request, CancellationToken cancellationToken = default) =>
    PostJsonAsync<PodCreateRequest, PodListEntryDto>("/libpod/pods/create", "Create pod", request, PodmanJsonContext.Default.PodCreateRequest, PodmanJsonContext.Default.PodListEntryDto, cancellationToken: cancellationToken);

  public Task<Result<List<PodListEntryDto>?>> ListPodsAsync(bool all = false, CancellationToken cancellationToken = default) =>
    GetJsonAsync<List<PodListEntryDto>>(
      "/libpod/pods/json",
      "List pods",
      PodmanJsonContext.Default.ListPodListEntryDto,
      [("all", all.ToString().ToLowerInvariant())],
      cancellationToken
    );

  public Task<Result<PodInspectDto?>> InspectPodAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<PodInspectDto>($"/libpod/pods/{Uri.EscapeDataString(name)}/json", "Inspect pod", PodmanJsonContext.Default.PodInspectDto, cancellationToken: cancellationToken);

  public Task<Result> PodExistsAsync(string name, CancellationToken cancellationToken = default) =>
    GetWithoutBodyAsync($"/libpod/pods/{Uri.EscapeDataString(name)}/exists", "Pod exists", cancellationToken: cancellationToken);

  public Task<Result> DeletePodAsync(string name, bool force = false, CancellationToken cancellationToken = default) =>
    DeleteWithoutBodyAsync(
      $"/libpod/pods/{Uri.EscapeDataString(name)}",
      "Delete pod",
      [("force", force.ToString().ToLowerInvariant())],
      cancellationToken
    );

  public Task<Result> StartPodAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"/libpod/pods/{Uri.EscapeDataString(name)}/start", "Start pod", cancellationToken: cancellationToken);

  public Task<Result> StopPodAsync(string name, int timeout = 10, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"/libpod/pods/{Uri.EscapeDataString(name)}/stop",
      "Stop pod",
      query: [("t", timeout.ToString())],
      cancellationToken: cancellationToken
    );

  public Task<Result> RestartPodAsync(string name, int timeout = 10, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"/libpod/pods/{Uri.EscapeDataString(name)}/restart",
      "Restart pod",
      query: [("t", timeout.ToString())],
      cancellationToken: cancellationToken
    );

  public Task<Result> KillPodAsync(string name, string? signal = null, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"/libpod/pods/{Uri.EscapeDataString(name)}/kill",
      "Kill pod",
      query: signal is null ? null : [("signal", signal)],
      cancellationToken: cancellationToken
    );

  public Task<Result> PausePodAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"/libpod/pods/{Uri.EscapeDataString(name)}/pause", "Pause pod", cancellationToken: cancellationToken);

  public Task<Result> UnpausePodAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"/libpod/pods/{Uri.EscapeDataString(name)}/unpause", "Unpause pod", cancellationToken: cancellationToken);

  public Task<Result<PruneReportDto?>> PrunePodsAsync(CancellationToken cancellationToken = default) =>
    PostLibpodAsync<PruneReportDto>("/libpod/pods/prune", "Prune pods", PodmanJsonContext.Default.PruneReportDto, cancellationToken: cancellationToken);

  public Task<Result<PodTopDto?>> TopPodAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<PodTopDto>($"/libpod/pods/{Uri.EscapeDataString(name)}/top", "Top pod", PodmanJsonContext.Default.PodTopDto, cancellationToken: cancellationToken);

  public Task<Result<PodStatsResponseDto?>> GetPodsStatsAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<PodStatsResponseDto>("/libpod/pods/stats", "Get pods stats", PodmanJsonContext.Default.PodStatsResponseDto, cancellationToken: cancellationToken);
}
