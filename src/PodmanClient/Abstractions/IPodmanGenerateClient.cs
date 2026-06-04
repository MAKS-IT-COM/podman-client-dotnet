using MaksIT.PodmanClientDotNet.Dtos.Generate;
using MaksIT.Results;

/// <summary>
/// Generate systemd units, kube YAML, and play-kube endpoints.
/// </summary>
public interface IPodmanGenerateClient {
  Task<Result<GenerateSystemdDto?>> GenerateSystemdAsync(
    string name,
    bool useName = false,
    bool createNew = false,
    int? restartSec = null,
    string? restartPolicy = null,
    string? containerPrefix = null,
    string? podPrefix = null,
    string? separator = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<string?>> GenerateKubeAsync(
    IEnumerable<string> names,
    bool service = false,
    CancellationToken cancellationToken = default
  );

  Task<Result<PlayKubeReportDto?>> PlayKubeAsync(
    Stream yaml,
    string? network = null,
    bool tlsVerify = true,
    bool start = true,
    string? logDriver = null,
    CancellationToken cancellationToken = default
  );
}
