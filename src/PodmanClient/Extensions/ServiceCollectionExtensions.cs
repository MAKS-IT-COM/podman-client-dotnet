using Microsoft.Extensions.DependencyInjection;

namespace MaksIT.PodmanClientDotNet.Extensions;

/// <summary>
/// Registers <see cref="PodmanClient"/> with <see cref="IHttpClientFactory"/> for dependency injection.
/// </summary>
public static class ServiceCollectionExtensions {
  /// <summary>
  /// Adds a typed <see cref="HttpClient"/> for <see cref="IPodmanClient"/> / <see cref="PodmanClient"/>.
  /// </summary>
  /// <param name="services">The service collection.</param>
  /// <param name="podmanClientConfiguration">Podman API settings (bound from configuration in the host application).</param>
  public static void AddPodmanClient(
    this IServiceCollection services,
    IPodmanClientConfiguration podmanClientConfiguration
  ) {
    ArgumentNullException.ThrowIfNull(podmanClientConfiguration);

    if (string.IsNullOrWhiteSpace(podmanClientConfiguration.ServerUrl))
      throw new ArgumentException(
        $"{nameof(IPodmanClientConfiguration.ServerUrl)} must be configured.",
        nameof(podmanClientConfiguration)
      );

    services.AddSingleton(podmanClientConfiguration);
    services.AddHttpClient<IPodmanClient, PodmanClient>();
  }
}
