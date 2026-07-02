using MaksIT.PodmanClientDotNet;
using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Container;
using MaksIT.Results;

public partial class PodmanClient {
  private static string ContainerPath(string name) => $"/libpod/containers/{Uri.EscapeDataString(name)}";

  public Task<Result<List<ContainerListEntryDto>?>> ListContainersAsync(
    bool all = false,
    int? limit = null,
    bool size = false,
    bool sync = false,
    string? filters = null,
    CancellationToken cancellationToken = default
  ) =>
    GetJsonAsync<List<ContainerListEntryDto>>(
      "/libpod/containers/json",
      "List containers",
      PodmanJsonContext.Default.ListContainerListEntryDto,
      [
        ("all", all.ToString().ToLowerInvariant()),
        ("limit", limit?.ToString()),
        ("size", size.ToString().ToLowerInvariant()),
        ("sync", sync.ToString().ToLowerInvariant()),
        ("filters", filters),
      ],
      cancellationToken
    );

  public Task<Result<ContainerInspectDto?>> InspectContainerAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ContainerInspectDto>($"{ContainerPath(name)}/json", "Inspect container", PodmanJsonContext.Default.ContainerInspectDto, cancellationToken: cancellationToken);

  public Task<Result> ContainerExistsAsync(string name, CancellationToken cancellationToken = default) =>
    GetWithoutBodyAsync($"{ContainerPath(name)}/exists", "Container exists", cancellationToken: cancellationToken);

  public Task<Result> RestartContainerAsync(string name, int timeout = 10, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"{ContainerPath(name)}/restart",
      "Restart container",
      query: [("t", timeout.ToString())],
      cancellationToken: cancellationToken
    );

  public Task<Result> KillContainerAsync(string name, string signal = "TERM", CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"{ContainerPath(name)}/kill",
      "Kill container",
      query: [("signal", signal)],
      cancellationToken: cancellationToken
    );

  public Task<Result> PauseContainerAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"{ContainerPath(name)}/pause", "Pause container", cancellationToken: cancellationToken);

  public Task<Result> UnpauseContainerAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"{ContainerPath(name)}/unpause", "Unpause container", cancellationToken: cancellationToken);

  public Task<Result<ContainerWaitDto?>> WaitContainerAsync(string name, string? condition = null, CancellationToken cancellationToken = default) =>
    PostLibpodAsync<ContainerWaitDto>(
      $"{ContainerPath(name)}/wait",
      "Wait container",
      PodmanJsonContext.Default.ContainerWaitDto,
      query: condition is null ? null : [("condition", condition)],
      cancellationToken: cancellationToken
    );

  public Task<Result<Stream?>> GetContainerLogsAsync(
    string name,
    bool follow = false,
    bool stdout = true,
    bool stderr = true,
    bool timestamps = false,
    string? since = null,
    string? until = null,
    string? tail = null,
    CancellationToken cancellationToken = default
  ) =>
    GetStreamAsync(
      $"{ContainerPath(name)}/logs",
      "Get container logs",
      [
        ("follow", follow.ToString().ToLowerInvariant()),
        ("stdout", stdout.ToString().ToLowerInvariant()),
        ("stderr", stderr.ToString().ToLowerInvariant()),
        ("timestamps", timestamps.ToString().ToLowerInvariant()),
        ("since", since),
        ("until", until),
        ("tail", tail),
      ],
      cancellationToken
    );

  public Task<Result<ContainerStatsDto?>> GetContainerStatsAsync(string name, bool stream = false, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ContainerStatsDto>(
      $"{ContainerPath(name)}/stats",
      "Get container stats",
      PodmanJsonContext.Default.ContainerStatsDto,
      [("stream", stream.ToString().ToLowerInvariant())],
      cancellationToken
    );

  public Task<Result<Dictionary<string, ContainerStatsDto>?>> GetContainersStatsAsync(
    IEnumerable<string>? containers = null,
    bool stream = false,
    CancellationToken cancellationToken = default
  ) {
    var query = new List<(string Key, string? Value)> { ("stream", stream.ToString().ToLowerInvariant()) };
    if (containers is not null) {
      foreach (var c in containers)
        query.Add(("containers", c));
    }

    return GetJsonAsync<Dictionary<string, ContainerStatsDto>>("/libpod/containers/stats", "Get containers stats", PodmanJsonContext.Default.DictionaryStringContainerStatsDto, query, cancellationToken);
  }

  public Task<Result<PruneReportDto?>> PruneContainersAsync(string? filters = null, CancellationToken cancellationToken = default) =>
    PostLibpodAsync<PruneReportDto>(
      "/libpod/containers/prune",
      "Prune containers",
      PodmanJsonContext.Default.PruneReportDto,
      query: filters is null ? null : [("filters", filters)],
      cancellationToken: cancellationToken
    );

  public Task<Result> RenameContainerAsync(string name, string newName, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"{ContainerPath(name)}/rename",
      "Rename container",
      query: [("name", newName)],
      cancellationToken: cancellationToken
    );

  public Task<Result> InitContainerAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"{ContainerPath(name)}/init", "Init container", cancellationToken: cancellationToken);

  public Task<Result<Stream?>> CheckpointContainerAsync(
    string name,
    bool keep = false,
    bool leaveRunning = false,
    bool tcpEstablished = false,
    bool export = false,
    bool ignoreRootFS = false,
    CancellationToken cancellationToken = default
  ) =>
    PostStreamAsync(
      $"{ContainerPath(name)}/checkpoint",
      "Checkpoint container",
      query: [
        ("keep", keep.ToString().ToLowerInvariant()),
        ("leaveRunning", leaveRunning.ToString().ToLowerInvariant()),
        ("tcpEstablished", tcpEstablished.ToString().ToLowerInvariant()),
        ("export", export.ToString().ToLowerInvariant()),
        ("ignoreRootFS", ignoreRootFS.ToString().ToLowerInvariant()),
      ],
      cancellationToken: cancellationToken
    );

  public Task<Result> RestoreContainerAsync(
    string name,
    string? importPath = null,
    bool keep = false,
    bool leaveRunning = false,
    bool tcpEstablished = false,
    bool ignoreRootFS = false,
    bool ignoreStaticIP = false,
    bool ignoreStaticMAC = false,
    CancellationToken cancellationToken = default
  ) =>
    PostWithoutBodyAsync(
      $"{ContainerPath(name)}/restore",
      "Restore container",
      query: [
        ("import", importPath),
        ("keep", keep.ToString().ToLowerInvariant()),
        ("leaveRunning", leaveRunning.ToString().ToLowerInvariant()),
        ("tcpEstablished", tcpEstablished.ToString().ToLowerInvariant()),
        ("ignoreRootFS", ignoreRootFS.ToString().ToLowerInvariant()),
        ("ignoreStaticIP", ignoreStaticIP.ToString().ToLowerInvariant()),
        ("ignoreStaticMAC", ignoreStaticMAC.ToString().ToLowerInvariant()),
      ],
      cancellationToken: cancellationToken
    );

  public Task<Result<ContainerMountDto?>> MountContainerAsync(string name, CancellationToken cancellationToken = default) =>
    PostLibpodAsync<ContainerMountDto>($"{ContainerPath(name)}/mount", "Mount container", PodmanJsonContext.Default.ContainerMountDto, cancellationToken: cancellationToken);

  public Task<Result> UnmountContainerAsync(string name, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync($"{ContainerPath(name)}/unmount", "Unmount container", cancellationToken: cancellationToken);

  public Task<Result<Stream?>> ExportContainerAsync(string name, CancellationToken cancellationToken = default) =>
    GetStreamAsync($"{ContainerPath(name)}/export", "Export container", cancellationToken: cancellationToken);

  public Task<Result<Stream?>> GetContainerArchiveAsync(string name, string path, CancellationToken cancellationToken = default) =>
    GetStreamAsync(
      $"{ContainerPath(name)}/archive",
      "Get container archive",
      [("path", path)],
      cancellationToken
    );

  public Task<Result> PutContainerArchiveAsync(
    string containerId,
    Stream tarStream,
    string path,
    bool pause = true,
    CancellationToken cancellationToken = default
  ) =>
    ExtractArchiveToContainerAsync(containerId, tarStream, path, pause);

  public Task<Result<Stream?>> AttachContainerAsync(
    string name,
    bool logs = false,
    bool stream = true,
    bool stdout = true,
    bool stderr = true,
    bool stdin = false,
    string? detachKeys = null,
    CancellationToken cancellationToken = default
  ) =>
    PostStreamAsync(
      $"{ContainerPath(name)}/attach",
      "Attach container",
      query: [
        ("logs", logs.ToString().ToLowerInvariant()),
        ("stream", stream.ToString().ToLowerInvariant()),
        ("stdout", stdout.ToString().ToLowerInvariant()),
        ("stderr", stderr.ToString().ToLowerInvariant()),
        ("stdin", stdin.ToString().ToLowerInvariant()),
        ("detachKeys", detachKeys),
      ],
      cancellationToken: cancellationToken
    );

  public Task<Result<ContainerChangesDto?>> GetContainerChangesAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ContainerChangesDto>($"{ContainerPath(name)}/changes", "Get container changes", PodmanJsonContext.Default.ContainerChangesDto, cancellationToken: cancellationToken);

  public Task<Result<ContainerCommitDto?>> CommitContainerAsync(
    string container,
    string? repo = null,
    string? tag = null,
    string? comment = null,
    string? author = null,
    bool pause = true,
    IEnumerable<string>? changes = null,
    string? format = null,
    CancellationToken cancellationToken = default
  ) {
    var query = new List<(string Key, string? Value)> {
      ("container", container),
      ("pause", pause.ToString().ToLowerInvariant()),
    };
    if (repo is not null) query.Add(("repo", repo));
    if (tag is not null) query.Add(("tag", tag));
    if (comment is not null) query.Add(("comment", comment));
    if (author is not null) query.Add(("author", author));
    if (format is not null) query.Add(("format", format));
    if (changes is not null) {
      foreach (var change in changes)
        query.Add(("changes", change));
    }

    return PostLibpodAsync<ContainerCommitDto>("/libpod/commit", "Commit container", PodmanJsonContext.Default.ContainerCommitDto, query: query, cancellationToken: cancellationToken);
  }

  public Task<Result<ContainerHealthCheckDto?>> HealthCheckContainerAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ContainerHealthCheckDto>($"{ContainerPath(name)}/healthcheck", "Health check container", PodmanJsonContext.Default.ContainerHealthCheckDto, cancellationToken: cancellationToken);

  public Task<Result<MountedContainersResponseDto?>> GetMountedContainersAsync(CancellationToken cancellationToken = default) =>
    GetJsonAsync<MountedContainersResponseDto>("/libpod/containers/showmounted", "Get mounted containers", PodmanJsonContext.Default.MountedContainersResponseDto, cancellationToken: cancellationToken);

  public Task<Result<ContainerTopDto?>> TopContainerAsync(
    string name,
    string psArgs = "-ef",
    bool stream = true,
    CancellationToken cancellationToken = default
  ) =>
    GetJsonAsync<ContainerTopDto>(
      $"{ContainerPath(name)}/top",
      "Top container",
      PodmanJsonContext.Default.ContainerTopDto,
      [
        ("ps_args", psArgs),
        ("stream", stream.ToString().ToLowerInvariant()),
      ],
      cancellationToken
    );
}
