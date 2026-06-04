namespace MaksIT.PodmanClientDotNet.Models.Volume;

/// <summary>
/// Libpod API request body for Create Volume request.
/// </summary>

public sealed class CreateVolumeRequest {
  public string? Name { get; set; }
  public Dictionary<string, string>? DriverOpts { get; set; }
  public Dictionary<string, string>? Labels { get; set; }
  public string? Driver { get; set; }
}
