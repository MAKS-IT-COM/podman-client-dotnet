namespace MaksIT.PodmanClientDotNet.Tests;

[Trait("Category", "Integration")]
public class PodmanClientImagesTests {
  private readonly IPodmanClient _client = PodmanClientTestFixture.CreateClient();

  #region Success Cases
  [Fact]
  public async Task PodmanClient_IntegrationTests() {
    await PullImageAsync_Should_Succeed();
    await TagImageAsync_Should_Succeed();
  }

  private async Task PullImageAsync_Should_Succeed() {
    var result = await _client.PullImageAsync("alpine:latest");
    PodmanClientTestFixture.AssertSuccess(result);
  }

  private async Task TagImageAsync_Should_Succeed() {
    var result = await _client.TagImageAsync("alpine:latest", "myrepo", "v1");
    PodmanClientTestFixture.AssertSuccess(result);
  }
  #endregion

  #region Fail Cases
  [Fact]
  public async Task PodmanClient_PullImage_Errors() {
    var result = await _client.PullImageAsync("dghdfdghmhgn:latest");
    PodmanClientTestFixture.AssertFailure(result);
  }

  [Fact]
  public async Task PodmanClient_TagImage_Errors() {
    var result = await _client.TagImageAsync("dghdfdghmhgn:latest", "myrepo", "v1");
    PodmanClientTestFixture.AssertFailure(result);
  }
  #endregion
}
