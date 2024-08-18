
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;


namespace MaksIT.PodmanClientDotNet.Tests {
  public class PodmanClientIntegrationTests {
    private readonly PodmanClient _client;

    public PodmanClientIntegrationTests() {
      // Initialize the logger
      var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
      var logger = loggerFactory.CreateLogger<PodmanClient>();

      // Initialize PodmanClient with real HttpClient
      _client = new PodmanClient(logger, "http://wks0002.corp.maks-it.com:8080", 60);
    }

    [Fact]
    public async Task PodmanClient_IntegrationTests() {
      // Test 1: Pull Image - Success
      await PullImageAsync_Should_Succeed();

      // Test 2: Tag Image - Success
      await TagImageAsync_Should_Succeed();
    }

    private async Task PullImageAsync_Should_Succeed() {
      // Arrange
      string imageReference = "alpine:latest"; // Example image

      // Act
      var exception = await Record.ExceptionAsync(() => _client.PullImageAsync(imageReference));

      // Assert
      Assert.Null(exception); // Expect no exceptions if the pull was successful
    }

    private async Task TagImageAsync_Should_Succeed() {
      // Arrange
      string image = "alpine:latest"; // Example image
      string repo = "myrepo";
      string tag = "v1";

      // Act
      var exception = await Record.ExceptionAsync(() => _client.TagImageAsync(image, repo, tag));

      // Assert
      Assert.Null(exception); // Expect no exceptions if the tagging was successful
    }

    [Fact]
    public async Task PodmanClient_PullImage_Errors() {

      // Arrange
      string imageReference = "dghdfdghmhgn:latest"; // Intentionally wrong image

      // Act
      var exception = await Record.ExceptionAsync(() => _client.PullImageAsync(imageReference));

      // Assert
      Assert.NotNull(exception); // Expect an exception due to nonexistent image
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }

    [Fact]
    public async Task PodmanClient_TagImage_Errors() {
      // Arrange
      string image = "dghdfdghmhgn:latest"; // Intentionally wrong image
      string repo = "myrepo";
      string tag = "v1";

      // Act
      var exception = await Record.ExceptionAsync(() => _client.TagImageAsync(image, repo, tag));

      // Assert
      Assert.NotNull(exception); // Expect an exception due to nonexistent image
      Assert.IsType<HttpRequestException>(exception); // Ensure it's the expected type
    }









  }
}
