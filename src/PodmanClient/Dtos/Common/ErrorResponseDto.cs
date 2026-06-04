namespace MaksIT.PodmanClientDotNet.Dtos.Common;
/// <summary>
/// Deserialized Podman libpod API payload (Error response).
/// </summary>

public sealed class ErrorResponseDto {
  public string? Cause { get; set; }
  public string? Message { get; set; }
}
