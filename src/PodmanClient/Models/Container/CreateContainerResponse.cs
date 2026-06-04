
namespace MaksIT.PodmanClientDotNet.Models.Container;

/// <summary>
/// Libpod API response body for Create Container response.
/// </summary>

public class CreateContainerResponse {
  public string? Id { get; set; }
  public string[]? Warnings { get; set; }
}