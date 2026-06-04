using System.Net.Http.Headers;

using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.Results;

public partial class PodmanClient {
  public async Task<Result<BuildReportDto?>> BuildImageAsync(
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
  ) {
    HttpContent? content = null;
    if (context is not null) {
      content = new StreamContent(context);
      content.Headers.ContentType = new MediaTypeHeaderValue("application/x-tar");
    }

    var streamResult = await PostStreamAsync(
      "/libpod/build",
      "Build image",
      content,
      [
        ("dockerfile", dockerfile),
        ("pull", pull.ToString().ToLowerInvariant()),
        ("rm", rm.ToString().ToLowerInvariant()),
        ("forcerm", forcerm.ToString().ToLowerInvariant()),
        ("nocache", nocache.ToString().ToLowerInvariant()),
        ("remote", remote),
        ("t", t),
        ("platform", platform),
        ("buildargs", buildargs),
        ("labels", labels),
      ],
      cancellationToken: cancellationToken
    ).ConfigureAwait(false);

    if (!streamResult.IsSuccess)
      return streamResult.ToResultOfType<BuildReportDto?>(null!);

    return await PodmanNdjsonStreams.DrainBuildAsync(streamResult.Value!, _logger, cancellationToken)
      .ConfigureAwait(false);
  }
}
