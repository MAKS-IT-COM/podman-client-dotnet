using System.Text;

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
  ) {
    var query = new List<(string Name, string? Value)> {
      ("all", all.ToString().ToLowerInvariant()),
    };
    if (!string.IsNullOrWhiteSpace(image))
      query.Add(("images", image));

    return PostLibpodAsync<ManifestCreateDto>(
      ManifestPath(name),
      "Create manifest",
      PodmanJsonContext.Default.ManifestCreateDto,
      query: [.. query],
      cancellationToken: cancellationToken
    );
  }

  public Task<Result> DeleteManifestAsync(string name, string? digest = null, CancellationToken cancellationToken = default) =>
    DeleteWithoutBodyAsync(
      ManifestPath(name),
      "Delete manifest",
      digest is null ? null : [("digest", digest)],
      cancellationToken
    );

  public Task<Result<ManifestInspectDto?>> InspectManifestAsync(string name, CancellationToken cancellationToken = default) =>
    GetJsonAsync<ManifestInspectDto>($"{ManifestPath(name)}/json", "Inspect manifest", PodmanJsonContext.Default.ManifestInspectDto, cancellationToken: cancellationToken);

  public Task<Result> AddToManifestAsync(string name, ManifestAddRequestDto request, CancellationToken cancellationToken = default) {
    var content = new StringContent(
      System.Text.Json.JsonSerializer.Serialize(request, PodmanJsonContext.Default.ManifestAddRequestDto),
      Encoding.UTF8,
      "application/json"
    );
    return PutWithoutBodyAsync(ManifestPath(name), "Add to manifest", content, cancellationToken: cancellationToken);
  }

  public Task<Result> PushManifestAsync(
    string name,
    string destination,
    bool all = false,
    CancellationToken cancellationToken = default
  ) =>
    PostWithoutBodyAsync(
      $"{ManifestPath(name)}/registry/{Uri.EscapeDataString(destination)}",
      "Push manifest",
      query: [
        ("all", all.ToString().ToLowerInvariant()),
      ],
      cancellationToken: cancellationToken
    );
}
