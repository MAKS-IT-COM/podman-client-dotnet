using System.Text.Json;

using MaksIT.PodmanClientDotNet.Models.Image;
using MaksIT.PodmanClientDotNet.Models;

namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {

    public async Task<List<ImagePullStatusResponse>> PullImageAsync(string reference, bool tlsVerify = true, bool quiet = false, string policy = "always", string arch = null, string os = null, string variant = null, bool allTags = false, string authHeader = null) {
      var query = $"reference={Uri.EscapeDataString(reference)}&tlsVerify={tlsVerify}&quiet={quiet}&policy={Uri.EscapeDataString(policy)}";
      if (!string.IsNullOrEmpty(arch)) query += $"&Arch={Uri.EscapeDataString(arch)}";
      if (!string.IsNullOrEmpty(os)) query += $"&OS={Uri.EscapeDataString(os)}";
      if (!string.IsNullOrEmpty(variant)) query += $"&Variant={Uri.EscapeDataString(variant)}";
      if (allTags) query += "&allTags=true";

      if (!string.IsNullOrEmpty(authHeader)) {
        _httpClient.DefaultRequestHeaders.Add("X-Registry-Auth", authHeader);
      }

      var response = await _httpClient.PostAsync($"/v1.41/libpod/images/pull?{query}", null);
      var imagePullResponses = new List<ImagePullStatusResponse>();

      if (response.IsSuccessStatusCode) {
        using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);
        string line;

        while ((line = await reader.ReadLineAsync()) != null) {
          if (line.Contains("\"status\"")) {
            // The line contains status information
            var statusResponse = JsonSerializer.Deserialize<ImagePullStatusResponse>(line);
            Console.WriteLine($"Status: {statusResponse.Status}");
          }
          else if (line.Contains("\"id\"") || line.Contains("\"images\"")) {
            // The line contains image ID information
            var imageResponse = JsonSerializer.Deserialize<ImagePullStatusResponse>(line);
            if (imageResponse != null) {
              imagePullResponses.Add(imageResponse);
            }
          }
        }

        return imagePullResponses;
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            Console.WriteLine($"Bad request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            Console.WriteLine($"Error pulling image: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
        return null;
      }
    }


    public async Task TagImageAsync(string image, string repo, string tag) {
      var response = await _httpClient.PostAsync($"/v1.41/libpod/images/{image}/tag?repo={Uri.EscapeDataString(repo)}&tag={Uri.EscapeDataString(tag)}", null);

      if (response.IsSuccessStatusCode) {
        if (response.StatusCode == System.Net.HttpStatusCode.Created) {
          Console.WriteLine("Image tagged successfully.");
        }
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            Console.WriteLine($"Bad parameter in request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such image: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.Conflict:
            Console.WriteLine($"Conflict error in operation: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            Console.WriteLine($"Error tagging image: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
      }
    }
  }
}
