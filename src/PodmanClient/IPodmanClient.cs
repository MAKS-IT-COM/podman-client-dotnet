/// <summary>
/// Podman REST API client contract.
/// </summary>
public interface IPodmanClient
  : IPodmanSystemClient,
    IPodmanContainersClient,
    IPodmanImagesClient,
    IPodmanVolumesClient,
    IPodmanNetworksClient,
    IPodmanPodsClient,
    IPodmanExecClient,
    IPodmanBuildClient,
    IPodmanManifestsClient,
    IPodmanGenerateClient { }
