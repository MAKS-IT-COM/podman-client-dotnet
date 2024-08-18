using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Models;
using MaksIT.PodmanClientDotNet.Models.Image;
using MaksIT.PodmanClientDotNet.Extensions;

namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {

    public async Task PullImageAsync(string reference, bool tlsVerify = true, bool quiet = false, string policy = "always", string arch = null, string os = null, string variant = null, bool allTags = false, string authHeader = null) {
      var query = $"reference={Uri.EscapeDataString(reference)}&tlsVerify={tlsVerify}&quiet={quiet}&policy={Uri.EscapeDataString(policy)}";
      if (!string.IsNullOrEmpty(arch)) query += $"&Arch={Uri.EscapeDataString(arch)}";
      if (!string.IsNullOrEmpty(os)) query += $"&OS={Uri.EscapeDataString(os)}";
      if (!string.IsNullOrEmpty(variant)) query += $"&Variant={Uri.EscapeDataString(variant)}";
      if (allTags) query += "&allTags=true";

      if (!string.IsNullOrEmpty(authHeader)) {
        _httpClient.DefaultRequestHeaders.Add("X-Registry-Auth", authHeader);
      }

      var response = await _httpClient.PostAsync($"/{_apiVersion}/libpod/images/pull?{query}", null);
      
      if (response.IsSuccessStatusCode) {

        var responseStream = await response.Content.ReadAsStreamAsync();
        using (var reader = new StreamReader(responseStream)) {
          string line;
          while ((line = await reader.ReadLineAsync()) != null) {
            if (line.StartsWith("{\"error\"")) {
              var errorDetails = line.ToObject<PullImageResponse>();
              _logger.LogError($"Error pulling image: {errorDetails?.Error}");
              throw new HttpRequestException($"Forced exception: {response.StatusCode} - {errorDetails?.Error}");
            }
          }
        }
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = errorContent.ToObject<ErrorResponse>();

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            _logger.LogError($"Bad request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            _logger.LogError($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            _logger.LogError($"Error pulling image: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
      }
    }


    public async Task TagImageAsync(string image, string repo, string tag) {
      var response = await _httpClient.PostAsync($"/{_apiVersion}/libpod/images/{image}/tag?repo={Uri.EscapeDataString(repo)}&tag={Uri.EscapeDataString(tag)}", null);

      if (response.IsSuccessStatusCode) {
        if (response.StatusCode == System.Net.HttpStatusCode.Created) {
          _logger.LogInformation("Image tagged successfully.");
        }
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = errorContent.ToObject<ErrorResponse>();

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            _logger.LogError($"Bad parameter in request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.NotFound:
            _logger.LogWarning($"No such image: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.Conflict:
            _logger.LogError($"Conflict error in operation: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            _logger.LogError($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            _logger.LogError($"Error tagging image: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
      }
    }
  }
}
