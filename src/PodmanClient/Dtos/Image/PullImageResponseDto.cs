namespace MaksIT.PodmanClientDotNet.Dtos.Image;
/// <summary>
/// Deserialized Podman libpod API payload (Pull Image response).
/// </summary>

public sealed class PullImageResponseDto {
  public string? Error { get; set; }
  public string? Id { get; set; }
  public string? Status { get; set; }
  public List<string>? Images { get; set; }
  public string? Stream { get; set; }
}
