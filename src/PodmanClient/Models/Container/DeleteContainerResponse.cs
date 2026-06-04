
namespace MaksIT.PodmanClientDotNet.Models.Container;

/// <summary>
/// Libpod API response body for Delete Container response.
/// </summary>

public class DeleteContainerResponse {
  public string? Err { get; set; }
  public string? Id { get; set; }
}