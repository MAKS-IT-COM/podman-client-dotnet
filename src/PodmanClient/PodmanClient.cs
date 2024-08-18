using Microsoft.Extensions.Logging;

namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {
    private readonly ILogger<PodmanClient> _logger;
    private readonly HttpClient _httpClient;

    private const string _apiVersion = "v1.41";

    /// <summary>
    /// Initializes a new instance of the <see cref="PodmanClient"/> class using the specified base address and timeout.
    /// </summary>
    /// <param name="logger">The logger instance used for logging within the client.</param>
    /// <param name="baseAddress">The base address of the Podman service.</param>
    /// <param name="timeOut">The timeout period in minutes for HTTP requests.</param>
    public PodmanClient(
        ILogger<PodmanClient> logger,
        string baseAddress,
        int timeOut = 60
    ) : this(
        logger,
        baseAddress,
        new HttpClient {
          Timeout = TimeSpan.FromMinutes(timeOut)
        }
    ) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PodmanClient"/> class using the provided <see cref="HttpClient"/> instance.
    /// </summary>
    /// <param name="logger">The logger instance used for logging within the client.</param>
    /// <param name="httpClient">An existing <see cref="HttpClient"/> instance configured for use with the Podman service.</param>
    /// <exception cref="ArgumentNullException">Thrown when the logger or httpClient parameter is null.</exception>
    public PodmanClient(
        ILogger<PodmanClient> logger,
        string serverUrl,
        HttpClient httpClient
    ) {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      if (serverUrl == null)
        throw new ArgumentNullException(nameof(serverUrl));

      _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

      ConfigureHttpClient(serverUrl);
    }

    /// <summary>
    /// Configures the default settings for the <see cref="HttpClient"/> used by this instance.
    /// Ensures that the "Accept" header is set to "application/json".
    /// </summary>
    private void ConfigureHttpClient(string baseAddress) {
      _httpClient.BaseAddress = new Uri(baseAddress);

      if (_httpClient.DefaultRequestHeaders.Contains("Accept"))
        _httpClient.DefaultRequestHeaders.Remove("Accept");

      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
  }
}
