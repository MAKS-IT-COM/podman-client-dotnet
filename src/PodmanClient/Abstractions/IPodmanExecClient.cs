using MaksIT.PodmanClientDotNet.Dtos.Exec;
using MaksIT.PodmanClientDotNet.Streaming;
using MaksIT.Results;

/// <summary>
/// Exec session create, start, resize, and inspect endpoints.
/// </summary>
public interface IPodmanExecClient {
  Task<Result<CreateExecResponseDto?>> CreateExecAsync(
    string containerName,
    string[] cmd,
    bool attachStderr = true,
    bool attachStdin = false,
    bool attachStdout = true,
    string? detachKeys = null,
    string[]? env = null,
    bool privileged = false,
    bool tty = false,
    string? user = null,
    string? workingDir = null
  );

  Task<Result> StartExecAsync(
    string execId,
    bool detach = false,
    bool tty = false,
    int? height = null,
    int? width = null
  );

  Task<Result<InspectExecResponseDto?>> InspectExecAsync(string execId);
  Task<Result> ResizeExecAsync(string execId, int height, int width, CancellationToken cancellationToken = default);

  /// <summary>
  /// Starts exec and returns a full-duplex attach session (requires <c>detach=false</c> on the wire).
  /// </summary>
  Task<Result<IPodmanAttachSession?>> StartExecSessionAsync(
    string execId,
    bool tty = false,
    int? height = null,
    int? width = null,
    CancellationToken cancellationToken = default
  );
}
