using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.PodmanClientDotNet.Streaming;
using MaksIT.Results;

/// <summary>
/// Image pull, push, list, inspect, tag, search, load, import, export, and prune endpoints.
/// </summary>
public interface IPodmanImagesClient {
  Task<Result> PullImageAsync(
    string reference,
    bool tlsVerify = true,
    bool quiet = false,
    string policy = "always",
    string? arch = null,
    string? os = null,
    string? variant = null,
    bool allTags = false,
    string? authHeader = null
  );

  Task<Result> TagImageAsync(string image, string repo, string tag);

  Task<Result<List<ImageListEntryDto>?>> ListImagesAsync(
    bool all = false,
    string? filters = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<ImageInspectDto?>> InspectImageAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> ImageExistsAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<ImageDeleteDto[]?>> DeleteImageAsync(string name, bool force = false, CancellationToken cancellationToken = default);
  Task<Result<ImageDeleteDto[]?>> RemoveImagesAsync(
    IEnumerable<string> images,
    bool all = false,
    bool force = false,
    CancellationToken cancellationToken = default
  );

  Task<Result<PruneReportDto?>> PruneImagesAsync(CancellationToken cancellationToken = default);
  Task<Result<List<ImageSearchResultDto>?>> SearchImagesAsync(string term, int? limit = null, CancellationToken cancellationToken = default);
  Task<Result> PushImageAsync(
    string name,
    string? destination = null,
    bool tlsVerify = true,
    bool compress = false,
    string? authHeader = null,
    CancellationToken cancellationToken = default
  );

  Task<Result> UntagImageAsync(string name, string? repo = null, string? tag = null, CancellationToken cancellationToken = default);
  Task<Result<List<ImageHistoryEntryDto>?>> GetImageHistoryAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<ImageTreeDto?>> GetImageTreeAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<ImageChangesDto?>> GetImageChangesAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<ImageImportDto?>> ImportImageAsync(
    Stream? tarball = null,
    string? changes = null,
    string? message = null,
    string? reference = null,
    string? url = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<ImageLoadDto?>> LoadImageAsync(Stream tarball, CancellationToken cancellationToken = default);
  Task<Result<Stream?>> ExportImagesAsync(
    IEnumerable<string> references,
    string? format = null,
    bool compress = false,
    CancellationToken cancellationToken = default
  );

  Task<Result<Stream?>> GetImageAsync(string name, string? format = null, bool compress = false, CancellationToken cancellationToken = default);

  Task<Result<IPodmanProgressSession<PullImageResponseDto>?>> PullImageWithProgressAsync(
    string reference,
    bool tlsVerify = true,
    bool quiet = false,
    string policy = "always",
    string? arch = null,
    string? os = null,
    string? variant = null,
    bool allTags = false,
    string? authHeader = null,
    CancellationToken cancellationToken = default
  );
}
