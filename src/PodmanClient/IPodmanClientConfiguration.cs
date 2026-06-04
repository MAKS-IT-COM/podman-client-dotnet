/// <summary>
/// Configuration for <see cref="PodmanClient"/> when registered via dependency injection.
/// Implement in the host application (for example an options class bound from <c>appsettings.json</c>).
/// </summary>
public interface IPodmanClientConfiguration {
  /// <summary>
  /// Recommended configuration section name (for example <c>PodmanClient</c> in <c>appsettings.json</c>).
  /// </summary>
  public const string SectionName = "PodmanClient";

  /// <summary>
  /// Base URL of the Podman API (for example <c>http://localhost:8080</c>).
  /// </summary>
  string ServerUrl { get; set; }

  /// <summary>
  /// Podman API version segment used in request paths. Defaults to <c>v1.41</c>.
  /// </summary>
  string ApiVersion { get; set; }

  /// <summary>
  /// HTTP request timeout in minutes.
  /// </summary>
  int TimeoutMinutes { get; set; }
}
