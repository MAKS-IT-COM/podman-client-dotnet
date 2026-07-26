using MaksIT.PodmanClientDotNet;
using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.System;
using MaksIT.Results;

public partial class PodmanClient {
  public Task<Result<LibpodPingDto?>> PingAsync(CancellationToken cancellationToken = default) =>
    SendAsync(
      () => _httpClient.GetAsync(LibpodPath("/libpod/_ping"), cancellationToken),
      "Ping",
      // Podman returns plain text "OK", not JSON.
      body => new LibpodPingDto {
        Ping = body.Trim().Equals("OK", StringComparison.OrdinalIgnoreCase)
      },
      cancellationToken
    );

  public Task<Result<LibpodVersionDto?>> GetVersionAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<LibpodVersionDto>("/libpod/version", "Get version", PodmanJsonContext.Default.LibpodVersionDto, cancellationToken: cancellationToken);

  public Task<Result<InfoDto?>> GetInfoAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<InfoDto>("/libpod/info", "Get info", PodmanJsonContext.Default.InfoDto, cancellationToken: cancellationToken);

  public Task<Result<SystemDfDto?>> GetSystemDiskUsageAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<SystemDfDto>("/libpod/system/df", "Get system disk usage", PodmanJsonContext.Default.SystemDfDto, cancellationToken: cancellationToken);

  public Task<Result<SystemPruneReportDto?>> PruneSystemAsync(CancellationToken cancellationToken = default) =>
    PostLibpodAsync<SystemPruneReportDto>("/libpod/system/prune", "Prune system", PodmanJsonContext.Default.SystemPruneReportDto, cancellationToken: cancellationToken);

  public Task<Result<Stream?>> GetEventsAsync(CancellationToken cancellationToken = default) =>
    GetStreamAsync("/libpod/events", "Get events", cancellationToken: cancellationToken);
}
