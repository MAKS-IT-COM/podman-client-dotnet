using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Streaming;
using MaksIT.Results;

/// <summary>
/// Image build endpoints (including streaming progress).
/// </summary>
public interface IPodmanBuildClient {
  Task<Result<BuildReportDto?>> BuildImageAsync(
    string dockerfile,
    Stream? context = null,
    bool pull = false,
    bool rm = true,
    bool forcerm = false,
    bool nocache = false,
    string? remote = null,
    string? t = null,
    string? platform = null,
    string? buildargs = null,
    string? labels = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<IPodmanProgressSession<BuildProgressLineDto>?>> BuildImageWithProgressAsync(
    string dockerfile,
    Stream? context = null,
    bool pull = false,
    bool rm = true,
    bool forcerm = false,
    bool nocache = false,
    string? remote = null,
    string? t = null,
    string? platform = null,
    string? buildargs = null,
    string? labels = null,
    CancellationToken cancellationToken = default
  );
}
