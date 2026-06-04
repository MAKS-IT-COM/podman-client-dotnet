using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.System;
using MaksIT.Results;

/// <summary>
/// Podman system endpoints: ping, version, info, events, disk usage, and prune.
/// </summary>
public interface IPodmanSystemClient {
  Task<Result<LibpodPingDto?>> PingAsync(CancellationToken cancellationToken = default);
  Task<Result<LibpodVersionDto?>> GetVersionAsync(CancellationToken cancellationToken = default);
  Task<Result<InfoDto?>> GetInfoAsync(CancellationToken cancellationToken = default);
  Task<Result<SystemDfDto?>> GetSystemDiskUsageAsync(CancellationToken cancellationToken = default);
  Task<Result<PruneReportDto?>> PruneSystemAsync(CancellationToken cancellationToken = default);
  Task<Result<Stream?>> GetEventsAsync(CancellationToken cancellationToken = default);
}
