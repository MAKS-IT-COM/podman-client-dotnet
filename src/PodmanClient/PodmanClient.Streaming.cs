using MaksIT.PodmanClientDotNet;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.PodmanClientDotNet.Models.Exec;
using MaksIT.PodmanClientDotNet.Streaming;
using MaksIT.Results;

public partial class PodmanClient {
  public async Task<Result<IPodmanAttachSession?>> AttachContainerSessionAsync(
    string name,
    bool logs = false,
    bool stream = true,
    bool stdout = true,
    bool stderr = true,
    bool stdin = true,
    bool tty = false,
    string? detachKeys = null,
    CancellationToken cancellationToken = default
  ) {
    try {
      var query = BuildQuery([
        ("logs", logs.ToString().ToLowerInvariant()),
        ("stream", stream.ToString().ToLowerInvariant()),
        ("stdout", stdout.ToString().ToLowerInvariant()),
        ("stderr", stderr.ToString().ToLowerInvariant()),
        ("stdin", stdin.ToString().ToLowerInvariant()),
        ("tty", tty.ToString().ToLowerInvariant()),
        ("detachKeys", detachKeys),
      ]);

      var hijack = await PodmanHijackConnection.ConnectAsync(
        GetServerBaseAddress(),
        _apiVersion,
        HttpMethod.Post,
        $"/libpod/containers/{Uri.EscapeDataString(name)}/attach",
        query,
        requestBody: null,
        cancellationToken
      ).ConfigureAwait(false);

      return Result<IPodmanAttachSession?>.Ok(new PodmanAttachSession(hijack, tty));
    }
    catch (Exception ex) {
      _logger.LogError(ex, "Attach container session failed for {Name}", name);
      return Result<IPodmanAttachSession?>.InternalServerError(null, ex.Message);
    }
  }

  public async Task<Result<IPodmanAttachSession?>> StartExecSessionAsync(
    string execId,
    bool tty = false,
    int? height = null,
    int? width = null,
    CancellationToken cancellationToken = default
  ) {
    try {
      var startExecRequest = new StartExecRequest {
        Detach = false,
        Tty = tty,
        Height = height,
        Width = width,
      };
      var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(startExecRequest, PodmanJsonContext.Default.StartExecRequest));
      var query = BuildQuery([]);

      var hijack = await PodmanHijackConnection.ConnectAsync(
        GetServerBaseAddress(),
        _apiVersion,
        HttpMethod.Post,
        $"/libpod/exec/{Uri.EscapeDataString(execId)}/start",
        query,
        body,
        cancellationToken
      ).ConfigureAwait(false);

      return Result<IPodmanAttachSession?>.Ok(new PodmanAttachSession(hijack, tty));
    }
    catch (Exception ex) {
      _logger.LogError(ex, "Start exec session failed for {ExecId}", execId);
      return Result<IPodmanAttachSession?>.InternalServerError(null, ex.Message);
    }
  }

  public async Task<Result<IPodmanProgressSession<PullImageResponseDto>?>> PullImageWithProgressAsync(
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
      "Pull image with progress",
      query: query,
      registryAuthHeader: authHeader,
      cancellationToken: cancellationToken
    ).ConfigureAwait(false);

    if (!streamResult.IsSuccess)
      return streamResult.ToResultOfType<IPodmanProgressSession<PullImageResponseDto>>(null!);

    return Result<IPodmanProgressSession<PullImageResponseDto>?>.Ok(
      new PodmanProgressSession<PullImageResponseDto>(streamResult.Value!, PodmanJsonContext.Default.PullImageResponseDto)
    );
  }

  public async Task<Result<IPodmanProgressSession<BuildProgressLineDto>?>> BuildImageWithProgressAsync(
    string dockerfile,
    Stream? context = null,
    bool pull = false,
    bool rm = true,
    bool forcerm = false,
    bool nocache = false,
    string? remote = null,
    string? t = null,
    string? platform = null,
    string? buildargs = null,
    string? labels = null,
    CancellationToken cancellationToken = default
  ) {
    HttpContent? content = null;
    if (context is not null) {
      content = new StreamContent(context);
      content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-tar");
    }

    var streamResult = await PostStreamAsync(
      "/libpod/build",
      "Build image with progress",
      content,
      [
        ("dockerfile", dockerfile),
        ("pull", pull.ToString().ToLowerInvariant()),
        ("rm", rm.ToString().ToLowerInvariant()),
        ("forcerm", forcerm.ToString().ToLowerInvariant()),
        ("nocache", nocache.ToString().ToLowerInvariant()),
        ("remote", remote),
        ("t", t),
        ("platform", platform),
        ("buildargs", buildargs),
        ("labels", labels),
      ],
      cancellationToken: cancellationToken
    ).ConfigureAwait(false);

    if (!streamResult.IsSuccess)
      return streamResult.ToResultOfType<IPodmanProgressSession<BuildProgressLineDto>>(null!);

    return Result<IPodmanProgressSession<BuildProgressLineDto>?>.Ok(
      new PodmanProgressSession<BuildProgressLineDto>(streamResult.Value!, PodmanJsonContext.Default.BuildProgressLineDto)
    );
  }

  private Uri GetServerBaseAddress() {
    if (_httpClient.BaseAddress is null)
      throw new InvalidOperationException("HttpClient BaseAddress is not configured.");
    return _httpClient.BaseAddress;
  }
}
