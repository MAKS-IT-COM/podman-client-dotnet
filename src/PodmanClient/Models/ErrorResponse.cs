
namespace MaksIT.PodmanClientDotNet.Models;

/// <summary>
/// Libpod API response body for Error response.
/// </summary>

public class ErrorResponse {
  public string? Cause { get; set; }
  public string? Message { get; set; }
  public int Response { get; set; }
}