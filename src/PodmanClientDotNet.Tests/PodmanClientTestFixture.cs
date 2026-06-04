using Microsoft.Extensions.Logging;

using MaksIT.Results;

namespace MaksIT.PodmanClientDotNet.Tests;

internal static class PodmanClientTestFixture {
  /// <summary>
  /// Podman API base URL for integration tests. Set <c>PODMAN_TEST_URL</c> (or <c>PODMAN_INTEGRATION_URL</c>) to enable.
  /// </summary>
  internal static string? IntegrationServerUrl =>
    Environment.GetEnvironmentVariable("PODMAN_TEST_URL")
    ?? Environment.GetEnvironmentVariable("PODMAN_INTEGRATION_URL");

  internal static bool IsIntegrationEnabled => !string.IsNullOrWhiteSpace(IntegrationServerUrl);

  internal static IPodmanClient CreateClient() {
    Assert.SkipUnless(IsIntegrationEnabled, "Set PODMAN_TEST_URL to run Podman integration tests.");
    var logger = LoggerFactory.Create(builder => builder.AddConsole())
      .CreateLogger<PodmanClient>();
    return new PodmanClient(logger, IntegrationServerUrl!, 60);
  }

  internal static void AssertSuccess(Result result) {
    Assert.True(result.IsSuccess, string.Join("; ", result.Messages));
  }

  internal static void AssertFailure(Result result) {
    Assert.False(result.IsSuccess);
    Assert.NotEmpty(result.Messages);
  }

  internal static void AssertSuccess<T>(Result<T?> result, Action<T?>? assertValue = null) {
    Assert.True(result.IsSuccess, string.Join("; ", result.Messages));
    assertValue?.Invoke(result.Value);
  }

  internal static void AssertFailure<T>(Result<T?> result) {
    Assert.False(result.IsSuccess);
    Assert.NotEmpty(result.Messages);
  }
}
