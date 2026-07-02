using MaksIT.PodmanClientDotNet;
using MaksIT.PodmanClientDotNet.Dtos.Manifest;
using MaksIT.Results;

public partial class PodmanClient {
  private static string ManifestPath(string name) => $"/libpod/manifests/{Uri.EscapeDataString(name)}";

  public Task<Result<ManifestCreateDto?>> CreateManifestAsync(
    string name,
    string? image = null,
    bool all = false,
    CancellationToken cancellationToken = default
  ) =>
    PostLibpodAsync<ManifestCreateDto>(
      "/libpod/manifests/create",
      "Create manifest",
      PodmanJsonContext.Default.ManifestCreateDto,
      query: [
        ("name", name),
        ("image", image),
        ("all", all.ToString().ToLowerInvariant()),
      ],
      cancellationToken: cancellationToken
    );

  public Task<Result> DeleteManifestAsync(string name, string? digest = null, CancellationToken cancellationToken = default) =>
    DeleteWithoutBodyAsync(
      ManifestPath(name),
      "Delete manifest",
      digest is null ? null : [("digest", digest)],
      cancellationToken
    );

  public Task<Result<ManifestInspectDto?>> InspectManifestAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ManifestInspectDto>($"{ManifestPath(name)}/json", "Inspect manifest", PodmanJsonContext.Default.ManifestInspectDto, cancellationToken: cancellationToken);

  public Task<Result> AddToManifestAsync(string name, ManifestAddRequestDto request, CancellationToken cancellationToken = default) =>
    PostJsonWithoutBodyAsync($"{ManifestPath(name)}/add", "Add to manifest", request, PodmanJsonContext.Default.ManifestAddRequestDto, cancellationToken: cancellationToken);

  public Task<Result> PushManifestAsync(
    string name,
    string destination,
    bool all = false,
    CancellationToken cancellationToken = default
  ) =>
    PostWithoutBodyAsync(
      $"/libpod/manifests/{Uri.EscapeDataString(name)}/push",
      "Push manifest",
      query: [
        ("destination", destination),
        ("all", all.ToString().ToLowerInvariant()),
      ],
      cancellationToken: cancellationToken
    );
}
