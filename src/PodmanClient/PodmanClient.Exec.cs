using MaksIT.PodmanClientDotNet;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.PodmanClientDotNet.Models;
using MaksIT.PodmanClientDotNet.Dtos.Exec;
using MaksIT.PodmanClientDotNet.Models.Exec;
using MaksIT.Results;

public partial class PodmanClient {
  public async Task<Result<CreateExecResponseDto?>> CreateExecAsync(
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
  ) {
    var execRequest = new CreateExecRequest {
      AttachStderr = attachStderr,
      AttachStdin = attachStdin,
      AttachStdout = attachStdout,
      Cmd = cmd,
      DetachKeys = detachKeys,
      Env = env,
      Privileged = privileged,
      Tty = tty,
      User = user,
      WorkingDir = workingDir,
    };

    return await PostJsonAsync<CreateExecRequest, CreateExecResponseDto>(
      $"/libpod/containers/{Uri.EscapeDataString(containerName)}/exec",
      "Create exec",
      execRequest,
      PodmanJsonContext.Default.CreateExecRequest,
      PodmanJsonContext.Default.CreateExecResponseDto
    ).ConfigureAwait(false);
  }

  public async Task<Result> StartExecAsync(
    string execId,
    bool detach = false,
    bool tty = false,
    int? height = null,
    int? width = null
  ) {
    var startExecRequest = new StartExecRequest {
      Detach = detach,
      Tty = tty,
      Height = height,
      Width = width,
    };

    var result = await PostJsonWithoutBodyAsync<StartExecRequest>(
      $"/libpod/exec/{Uri.EscapeDataString(execId)}/start",
      "Start exec",
      startExecRequest,
      PodmanJsonContext.Default.StartExecRequest
    ).ConfigureAwait(false);

    if (result.IsSuccess)
      _logger.LogInformation("Exec started successfully.");

    return result;
  }

  public Task<Result<InspectExecResponseDto?>> InspectExecAsync(string execId) =>
    GetJsonAsync<InspectExecResponseDto>(
      $"/libpod/exec/{Uri.EscapeDataString(execId)}/json",
      "Inspect exec",
      PodmanJsonContext.Default.InspectExecResponseDto
    );

  public Task<Result> ResizeExecAsync(string execId, int height, int width, CancellationToken cancellationToken = default) =>
    PostWithoutBodyAsync(
      $"/libpod/exec/{Uri.EscapeDataString(execId)}/resize",
      "Resize exec",
      query: [
        ("h", height.ToString()),
        ("w", width.ToString()),
      ],
      cancellationToken: cancellationToken
    );
}
