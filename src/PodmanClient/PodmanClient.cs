using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Extensions;

/// <summary>
/// HTTP client for the Podman REST API.
/// </summary>
public partial class PodmanClient : IPodmanClient {
  private readonly ILogger<PodmanClient> _logger;
  private readonly HttpClient _httpClient;
  private readonly string _apiVersion;

  /// <summary>
  /// Initializes a new instance using a dedicated <see cref="HttpClient"/> (not from <see cref="IHttpClientFactory"/>).
  /// </summary>
  public PodmanClient(
    ILogger<PodmanClient> logger,
    string baseAddress,
    int timeOut = 60
  ) : this(
    logger,
    baseAddress,
    new HttpClient { Timeout = TimeSpan.FromMinutes(timeOut) }
  ) { }

  /// <summary>
  /// Initializes a new instance with an existing <see cref="HttpClient"/> and explicit server URL.
  /// </summary>
  public PodmanClient(
    ILogger<PodmanClient> logger,
    string serverUrl,
    HttpClient httpClient
  ) {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _apiVersion = "v1.41";
    ConfigureHttpClient(serverUrl);
  }

  /// <summary>
  /// Initializes a typed client instance resolved via <see cref="ServiceCollectionExtensions.AddPodmanClient"/>.
  /// </summary>
  public PodmanClient(
    HttpClient httpClient,
    ILogger<PodmanClient> logger,
    IPodmanClientConfiguration configuration
  ) {
    ArgumentNullException.ThrowIfNull(configuration);

    if (string.IsNullOrWhiteSpace(configuration.ServerUrl))
      throw new ArgumentException(
        $"{nameof(IPodmanClientConfiguration.ServerUrl)} must be configured.",
        nameof(configuration)
      );

    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _apiVersion = string.IsNullOrWhiteSpace(configuration.ApiVersion)
      ? "v1.41"
      : configuration.ApiVersion;
    _httpClient.Timeout = TimeSpan.FromMinutes(Math.Max(1, configuration.TimeoutMinutes));
    ConfigureHttpClient(configuration.ServerUrl);
  }

  private void ConfigureHttpClient(string baseAddress) {
    _httpClient.BaseAddress = new Uri(baseAddress.TrimEnd('/') + "/");

    if (_httpClient.DefaultRequestHeaders.Contains("Accept"))
      _httpClient.DefaultRequestHeaders.Remove("Accept");

    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
  }
}
