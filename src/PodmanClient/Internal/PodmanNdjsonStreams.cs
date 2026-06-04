using Microsoft.Extensions.Logging;

using MaksIT.Core.Extensions;
using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.Results;

namespace MaksIT.PodmanClientDotNet.Internal;

internal static class PodmanNdjsonStreams {
  public static async Task<Result> DrainPullOrPushAsync(
    Stream stream,
    ILogger logger,
    string operation,
    CancellationToken cancellationToken = default
  ) {
    await using var owned = stream;
    using var reader = new StreamReader(stream, leaveOpen: false);

    while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line) {
      if (string.IsNullOrWhiteSpace(line))
        continue;

      if (!line.Contains("\"error\"", StringComparison.Ordinal))
        continue;

      var errorDetails = line.ToObject<PullImageResponseDto>();
      var message = errorDetails?.Error ?? $"{operation} failed.";
      logger.LogError("{Operation} failed: {Message}", operation, message);
      return Result.BadRequest(message);
    }

    return Result.Ok($"{operation} completed successfully.");
  }

  public static async Task<Result<BuildReportDto?>> DrainBuildAsync(
    Stream stream,
    ILogger logger,
    CancellationToken cancellationToken = default
  ) {
    await using var owned = stream;
    using var reader = new StreamReader(stream, leaveOpen: false);
    BuildReportDto? report = null;

    while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line) {
      if (string.IsNullOrWhiteSpace(line))
        continue;

      var progress = line.ToObject<BuildProgressLineDto>();
      if (progress is null)
        continue;

      if (!string.IsNullOrWhiteSpace(progress.Error)) {
        logger.LogError("Build image failed: {Message}", progress.Error);
        return Result<BuildReportDto?>.BadRequest(null, progress.Error);
      }

      if (!string.IsNullOrWhiteSpace(progress.Id))
        report = new BuildReportDto { Id = progress.Id, Names = report?.Names };
    }

    return Result<BuildReportDto?>.Ok(report, "Image built successfully.");
  }
}
