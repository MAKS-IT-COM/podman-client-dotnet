using Microsoft.Extensions.Logging.Abstractions;

namespace MaksIT.PodmanClientDotNet.PowerShell;

/// <summary>Holds the current Podman client for the PowerShell session.</summary>
internal static class PodmanConnectionState {
  private static readonly Lock Lock = new();
  private static HttpClient? _httpClient;

  public static IPodmanClient? Client { get; private set; }

  public static void SetConnection(string baseAddress, int timeoutMinutes = 60, string? apiVersion = null) {
    lock (Lock) {
      _httpClient?.Dispose();
      _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(Math.Max(1, timeoutMinutes)) };
      var configuration = new PodmanClientSessionConfiguration {
        ServerUrl = baseAddress,
        ApiVersion = string.IsNullOrWhiteSpace(apiVersion) ? "v5.4.0" : apiVersion,
        TimeoutMinutes = timeoutMinutes
      };
      Client = new PodmanClient(_httpClient, NullLogger<PodmanClient>.Instance, configuration);
    }
  }

  public static void ClearConnection() {
    lock (Lock) {
      Client = null;
      _httpClient?.Dispose();
      _httpClient = null;
    }
  }

  private sealed class PodmanClientSessionConfiguration : IPodmanClientConfiguration {
    public string ServerUrl { get; set; } = "";
    public string ApiVersion { get; set; } = "v5.4.0";
    public int TimeoutMinutes { get; set; } = 60;
  }
}
