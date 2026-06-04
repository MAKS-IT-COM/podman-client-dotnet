using System.Net.Http.Headers;

using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.Results;

public partial class PodmanClient {
  private static string ImagePath(string name) => $"/libpod/images/{Uri.EscapeDataString(name)}";

  public Task<Result<List<ImageListEntryDto>?>> ListImagesAsync(
    bool all = false,
    string? filters = null,
    CancellationToken cancellationToken = default
  ) =>
    GetJsonAsync<List<ImageListEntryDto>>(
      "/libpod/images/json",
      "List images",
      [
        ("all", all.ToString().ToLowerInvariant()),
        ("filters", filters),
      ],
      cancellationToken
    );

  public Task<Result<ImageInspectDto?>> InspectImageAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ImageInspectDto>($"{ImagePath(name)}/json", "Inspect image", cancellationToken: cancellationToken);

  public Task<Result> ImageExistsAsync(string name, CancellationToken cancellationToken = default) =>
    GetWithoutBodyAsync($"{ImagePath(name)}/exists", "Image exists", cancellationToken: cancellationToken);

  public Task<Result<ImageDeleteDto[]?>> DeleteImageAsync(string name, bool force = false, CancellationToken cancellationToken = default) =>
    DeleteJsonAsync<ImageDeleteDto[]>(
      ImagePath(name),
      "Delete image",
      [("force", force.ToString().ToLowerInvariant())],
      cancellationToken
    );

  public Task<Result<ImageDeleteDto[]?>> RemoveImagesAsync(
    IEnumerable<string> images,
    bool all = false,
    bool force = false,
    CancellationToken cancellationToken = default
  ) {
    var query = new List<(string Key, string? Value)> {
      ("all", all.ToString().ToLowerInvariant()),
      ("force", force.ToString().ToLowerInvariant()),
    };
    foreach (var image in images)
      query.Add(("images", image));

    return DeleteJsonAsync<ImageDeleteDto[]>("/libpod/images/remove", "Remove images", query, cancellationToken);
  }

  public Task<Result<PruneReportDto?>> PruneImagesAsync(CancellationToken cancellationToken = default) =>
    PostLibpodAsync<PruneReportDto>("/libpod/images/prune", "Prune images", cancellationToken: cancellationToken);

  public Task<Result<List<ImageSearchResultDto>?>> SearchImagesAsync(
    string term,
    int? limit = null,
    CancellationToken cancellationToken = default
  ) =>
    GetJsonAsync<List<ImageSearchResultDto>>(
      "/libpod/images/search",
      "Search images",
      [
        ("term", term),
        ("limit", limit?.ToString()),
      ],
      cancellationToken
    );

  public async Task<Result> PushImageAsync(
    string name,
    string? destination = null,
    bool tlsVerify = true,
    bool compress = false,
    string? authHeader = null,
    CancellationToken cancellationToken = default
  ) {
    var streamResult = await PostStreamAsync(
      $"{ImagePath(name)}/push",
      "Push image",
      query: [
        ("destination", destination),
        ("tlsVerify", tlsVerify.ToString().ToLowerInvariant()),
        ("compress", compress.ToString().ToLowerInvariant()),
      ],
      registryAuthHeader: authHeader,
      cancellationToken: cancellationToken
    ).ConfigureAwait(false);

    if (!streamResult.IsSuccess)
      return streamResult.ToResult();

    return await PodmanNdjsonStreams.DrainPullOrPushAsync(streamResult.Value!, _logger, "Push image", cancellationToken)
      .ConfigureAwait(false);
  }

  public Task<Result> UntagImageAsync(
    string name,
    string? repo = null,
    string? tag = null,
    CancellationToken cancellationToken = default
  ) =>
    PostWithoutBodyAsync(
      $"{ImagePath(name)}/untag",
      "Untag image",
      query: [
        ("repo", repo),
        ("tag", tag),
      ],
      cancellationToken: cancellationToken
    );

  public Task<Result<List<ImageHistoryEntryDto>?>> GetImageHistoryAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<List<ImageHistoryEntryDto>>($"{ImagePath(name)}/history", "Get image history", cancellationToken: cancellationToken);

  public Task<Result<ImageTreeDto?>> GetImageTreeAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ImageTreeDto>($"{ImagePath(name)}/tree", "Get image tree", cancellationToken: cancellationToken);

  public Task<Result<ImageChangesDto?>> GetImageChangesAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ImageChangesDto>($"{ImagePath(name)}/changes", "Get image changes", cancellationToken: cancellationToken);

  public Task<Result<ImageImportDto?>> ImportImageAsync(
    Stream? tarball = null,
    string? changes = null,
    string? message = null,
    string? reference = null,
    string? url = null,
    CancellationToken cancellationToken = default
  ) {
    HttpContent? content = null;
    if (tarball is not null) {
      content = new StreamContent(tarball);
      content.Headers.ContentType = new MediaTypeHeaderValue("application/x-tar");
    }

    return PostLibpodAsync<ImageImportDto>(
      "/libpod/images/import",
      "Import image",
      content,
      [
        ("changes", changes),
        ("message", message),
        ("reference", reference),
        ("url", url),
      ],
      cancellationToken
    );
  }

  public Task<Result<ImageLoadDto?>> LoadImageAsync(Stream tarball, CancellationToken cancellationToken = default) {
    var content = new StreamContent(tarball);
    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-tar");
    return PostLibpodAsync<ImageLoadDto>("/libpod/images/load", "Load image", content, cancellationToken: cancellationToken);
  }

  public Task<Result<Stream?>> ExportImagesAsync(
    IEnumerable<string> references,
    string? format = null,
    bool compress = false,
    CancellationToken cancellationToken = default
  ) {
    var query = new List<(string Key, string? Value)> {
      ("compress", compress.ToString().ToLowerInvariant()),
      ("format", format),
    };
    foreach (var reference in references)
      query.Add(("references", reference));

    return GetStreamAsync("/libpod/images/export", "Export images", query, cancellationToken);
  }

  public Task<Result<Stream?>> GetImageAsync(
    string name,
    string? format = null,
    bool compress = false,
    CancellationToken cancellationToken = default
  ) =>
    GetStreamAsync(
      $"{ImagePath(name)}/get",
      "Get image",
      [
        ("format", format),
        ("compress", compress.ToString().ToLowerInvariant()),
      ],
      cancellationToken
    );
}
