using System.Net.Http.Headers;

using MaksIT.PodmanClientDotNet.Dtos.Generate;
using MaksIT.Results;

public partial class PodmanClient {
  public Task<Result<GenerateSystemdDto?>> GenerateSystemdAsync(
    string name,
    bool useName = false,
    bool createNew = false,
    int? restartSec = null,
    string? restartPolicy = null,
    string? containerPrefix = null,
    string? podPrefix = null,
    string? separator = null,
    CancellationToken cancellationToken = default
  ) =>
    GetJsonAsync<GenerateSystemdDto>(
      $"/libpod/generate/{Uri.EscapeDataString(name)}/systemd",
      "Generate systemd",
      [
        ("useName", useName.ToString().ToLowerInvariant()),
        ("new", createNew.ToString().ToLowerInvariant()),
        ("restartSec", restartSec?.ToString()),
        ("restartPolicy", restartPolicy),
        ("containerPrefix", containerPrefix),
        ("podPrefix", podPrefix),
        ("separator", separator),
      ],
      cancellationToken
    );

  public Task<Result<string?>> GenerateKubeAsync(
    IEnumerable<string> names,
    bool service = false,
    CancellationToken cancellationToken = default
  ) {
    var query = new List<(string Key, string? Value)> {
      ("service", service.ToString().ToLowerInvariant()),
    };
    foreach (var name in names)
      query.Add(("names", name));

    return SendAsync<string>(
      () => _httpClient.GetAsync(LibpodPath("/libpod/generate/kube") + BuildQuery(query), cancellationToken),
      "Generate kube",
      body => string.IsNullOrWhiteSpace(body) ? null : body.Trim('"'),
      cancellationToken
    );
  }

  public Task<Result<PlayKubeReportDto?>> PlayKubeAsync(
    Stream yaml,
    string? network = null,
    bool tlsVerify = true,
    bool start = true,
    string? logDriver = null,
    CancellationToken cancellationToken = default
  ) {
    var content = new StreamContent(yaml);
    content.Headers.ContentType = new MediaTypeHeaderValue("application/yaml");
    return PostLibpodAsync<PlayKubeReportDto>(
      "/libpod/play/kube",
      "Play kube",
      content,
      [
        ("network", network),
        ("tlsVerify", tlsVerify.ToString().ToLowerInvariant()),
        ("start", start.ToString().ToLowerInvariant()),
        ("logDriver", logDriver),
      ],
      cancellationToken
    );
  }
}
