namespace MaksIT.PodmanClientDotNet.Dtos.Build;

/// <summary>
/// A single NDJSON line from the image build stream.
/// </summary>
public sealed class BuildProgressLineDto {
  public string? Stream { get; set; }
  public string? Error { get; set; }
  public string? Status { get; set; }
  public string? Id { get; set; }
}
