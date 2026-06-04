using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.Results;

using Microsoft.Extensions.Logging;

public partial class PodmanClient {
  public async Task<Result> PullImageAsync(
    string reference,
    bool tlsVerify = true,
    bool quiet = false,
    string policy = "always",
    string? arch = null,
    string? os = null,
    string? variant = null,
    bool allTags = false,
    string? authHeader = null
  ) {
    var query = new List<(string Key, string? Value)> {
      ("reference", reference),
      ("tlsVerify", tlsVerify.ToString().ToLowerInvariant()),
      ("quiet", quiet.ToString().ToLowerInvariant()),
      ("policy", policy),
      ("arch", arch),
      ("OS", os),
      ("Variant", variant),
    };
    if (allTags)
      query.Add(("allTags", "true"));

    var streamResult = await PostStreamAsync(
      "/libpod/images/pull",
      "Pull image",
      query: query,
      registryAuthHeader: authHeader
    ).ConfigureAwait(false);

    if (!streamResult.IsSuccess)
      return streamResult.ToResult();

    return await PodmanNdjsonStreams.DrainPullOrPushAsync(streamResult.Value!, _logger, "Pull image")
      .ConfigureAwait(false);
  }

  public async Task<Result> TagImageAsync(string image, string repo, string tag) {
    var response = await _httpClient.PostAsync(
      $"/{_apiVersion}/libpod/images/{image}/tag?repo={Uri.EscapeDataString(repo)}&tag={Uri.EscapeDataString(tag)}",
      null
    );

    using (response) {
      if (response.IsSuccessStatusCode) {
        _logger.LogInformation("Image tagged successfully.");
        return response.StatusCode == System.Net.HttpStatusCode.Created
          ? Result.Created("Image tagged successfully.")
          : Result.Ok("Image tagged successfully.");
      }

      var errorContent = await response.Content.ReadAsStringAsync();
      var errorMessage = PodmanHttpResults.GetErrorMessage(errorContent);
      PodmanHttpResults.LogFailure(_logger, response.StatusCode, "Tag image", errorMessage);
      return PodmanHttpResults.Failure(response.StatusCode, errorMessage);
    }
  }
}
