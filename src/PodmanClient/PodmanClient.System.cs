using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.System;
using MaksIT.Results;

public partial class PodmanClient {
  public Task<Result<LibpodPingDto?>> PingAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<LibpodPingDto>("/libpod/_ping", "Ping", cancellationToken: cancellationToken);

  public Task<Result<LibpodVersionDto?>> GetVersionAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<LibpodVersionDto>("/libpod/version", "Get version", cancellationToken: cancellationToken);

  public Task<Result<InfoDto?>> GetInfoAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<InfoDto>("/libpod/info", "Get info", cancellationToken: cancellationToken);

  public Task<Result<SystemDfDto?>> GetSystemDiskUsageAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<SystemDfDto>("/libpod/system/df", "Get system disk usage", cancellationToken: cancellationToken);

  public Task<Result<PruneReportDto?>> PruneSystemAsync(CancellationToken cancellationToken = default) =>
    PostLibpodAsync<PruneReportDto>("/libpod/system/prune", "Prune system", cancellationToken: cancellationToken);

  public Task<Result<Stream?>> GetEventsAsync(CancellationToken cancellationToken = default) =>
    GetStreamAsync("/libpod/events", "Get events", cancellationToken: cancellationToken);
}
