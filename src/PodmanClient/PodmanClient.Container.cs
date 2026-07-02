using MaksIT.PodmanClientDotNet;
using System.Net;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.PodmanClientDotNet.Models;
using MaksIT.PodmanClientDotNet.Dtos.Container;
using MaksIT.PodmanClientDotNet.Models.Container;
using MaksIT.PodmanClientDotNet.Models.Exec;
using MaksIT.Results;

public partial class PodmanClient {
  public async Task<Result<CreateContainerResponseDto?>> CreateContainerAsync(
    string name,
    string image,
    List<string>? command = null,
    Dictionary<string, string>? env = null,
    bool? remove = null,
    bool? stdin = null,
    bool? terminal = null,
    List<Mount>? mounts = null,
    bool? privileged = null,
    string? hostname = null,
    Namespace? netns = null,
    List<PortMapping>? portMappings = null,
    string? restartPolicy = null,
    ulong? stopTimeout = null,
    List<string>? capAdd = null,
    List<string>? capDrop = null,
    List<string>? dnsServers = null,
    List<string>? dnsSearch = null,
    List<string>? dnsOptions = null,
    bool? publishImagePorts = null,
    List<string>? cniNetworks = null,
    Dictionary<string, string>? labels = null,
    bool? readOnlyFilesystem = null,
    List<POSIXRlimit>? rLimits = null,
    List<LinuxDevice>? devices = null,
    string? ociRuntime = null,
    string? pod = null,
    bool? noNewPrivileges = null,
    string? cgroupsMode = null,
    Dictionary<string, string>? storageOpts = null,
    bool? unsetenvall = null,
    Dictionary<string, string>? secretEnv = null,
    string? timezone = null,
    Dictionary<string, string>? sysctl = null,
    string? seccompProfilePath = null,
    string? seccompPolicy = null,
    Dictionary<string, string>? annotations = null,
    string? apparmorProfile = null,
    string? baseHostsFile = null,
    string? cgroupParent = null,
    Namespace? cgroupns = null,
    List<string>? chrootDirectories = null,
    string? conmonPidFile = null,
    List<string>? containerCreateCommand = null,
    bool? createWorkingDir = null,
    List<string>? dependencyContainers = null,
    List<LinuxDeviceCgroup>? deviceCgroupRule = null,
    List<string>? devicesFrom = null,
    List<string>? entrypoint = null,
    bool? envHost = null,
    List<string>? envMerge = null,
    Dictionary<ushort, string>? expose = null,
    string? groupEntry = null,
    List<string>? groups = null,
    long? healthCheckOnFailureAction = null,
    Schema2HealthConfig? healthConfig = null,
    List<LinuxDevice>? hostDeviceList = null,
    List<string>? hostAdd = null,
    List<string>? hostUsers = null,
    bool? envHTTPProxy = null,
    IDMappingOptions? idMappings = null,
    string? imageArch = null,
    string? imageOS = null,
    string? imageVariant = null,
    string? imageVolumeMode = null,
    List<ImageVolume>? imageVolumes = null,
    bool? init = null,
    string? initContainerType = null,
    string? initPath = null,
    LinuxIntelRdt? intelRdt = null,
    Namespace? ipcns = null,
    bool? labelNested = null,
    LogConfigLibpod? logConfiguration = null,
    bool? managePassword = null,
    List<string>? mask = null,
    Dictionary<string, string>? networkOptions = null,
    Dictionary<string, NetworkSettings>? networks = null,
    long? oomScoreAdj = null,
    List<OverlayVolume>? overlayVolumes = null,
    string? passwdEntry = null,
    LinuxPersonality? personality = null,
    Namespace? pidns = null,
    string? rawImageName = null,
    bool? readWriteTmpfs = null,
    LinuxResources? resourceLimits = null,
    ulong? restartTries = null,
    string? rootfs = null,
    string? rootfsMapping = null,
    bool? rootfsOverlay = null,
    string? rootfsPropagation = null,
    string? sdnotifyMode = null,
    List<SecretProp>? secrets = null,
    List<string>? selinuxOpts = null,
    long? shmSize = null,
    long? shmSizeSystemd = null,
    StartupHealthConfig? startupHealthConfig = null,
    long? stopSignal = null,
    string? systemd = null,
    Dictionary<string, ulong>? throttleReadBpsDevice = null,
    Dictionary<string, ulong>? throttleReadIopsDevice = null,
    Dictionary<string, ulong>? throttleWriteBpsDevice = null,
    Dictionary<string, ulong>? throttleWriteIopsDevice = null,
    ulong? timeout = null,
    string? umask = null,
    Dictionary<string, string>? unified = null,
    List<string>? unmask = null,
    bool? useImageHosts = null,
    bool? useImageResolvConf = null,
    string? user = null,
    Namespace? userns = null,
    Namespace? utsns = null,
    bool? volatileFlag = null,
    List<NamedVolume>? volumes = null,
    List<string>? volumesFrom = null,
    Dictionary<string, ulong>? weightDevice = null,
    string? workDir = null
  ) {
    var createContainerParameters = new CreateContainerRequest {
      Name = name,
      Image = image,
      Command = command,
      Env = env,
      WorkDir = workDir,
      Remove = remove,
      Stdin = stdin,
      Terminal = terminal,
      Mounts = mounts,
      Privileged = privileged,
      Hostname = hostname,
      Netns = netns,
      Portmappings = portMappings,
      RestartPolicy = restartPolicy,
      StopTimeout = stopTimeout,
      CapAdd = capAdd,
      CapDrop = capDrop,
      DNSServer = dnsServers,
      DNSSearch = dnsSearch,
      DNSOption = dnsOptions,
      PublishImagePorts = publishImagePorts,
      CNINetworks = cniNetworks,
      Labels = labels,
      ReadOnlyFilesystem = readOnlyFilesystem,
      RLimits = rLimits,
      Devices = devices,
      OciRuntime = ociRuntime,
      Pod = pod,
      NoNewPrivileges = noNewPrivileges,
      CgroupsMode = cgroupsMode,
      StorageOpts = storageOpts,
      Unmask = unmask,
      Unsetenvall = unsetenvall,
      SecretEnv = secretEnv,
      Timezone = timezone,
      Sysctl = sysctl,
      SeccompProfilePath = seccompProfilePath,
      SeccompPolicy = seccompPolicy,
      Annotations = annotations,
      ApparmorProfile = apparmorProfile,
      BaseHostsFile = baseHostsFile,
      CgroupParent = cgroupParent,
      Cgroupns = cgroupns,
      ChrootDirectories = chrootDirectories,
      ConmonPidFile = conmonPidFile,
      ContainerCreateCommand = containerCreateCommand,
      CreateWorkingDir = createWorkingDir,
      DependencyContainers = dependencyContainers,
      DeviceCgroupRule = deviceCgroupRule,
      DevicesFrom = devicesFrom,
      Entrypoint = entrypoint,
      EnvHost = envHost,
      EnvMerge = envMerge,
      Expose = expose,
      GroupEntry = groupEntry,
      Groups = groups,
      HealthCheckOnFailureAction = healthCheckOnFailureAction,
      HealthConfig = healthConfig,
      HostDeviceList = hostDeviceList,
      HostAdd = hostAdd,
      HostUsers = hostUsers,
      EnvHTTPProxy = envHTTPProxy,
      IDMappings = idMappings,
      ImageArch = imageArch,
      ImageOS = imageOS,
      ImageVariant = imageVariant,
      ImageVolumeMode = imageVolumeMode,
      ImageVolumes = imageVolumes,
      Init = init,
      InitContainerType = initContainerType,
      InitPath = initPath,
      IntelRdt = intelRdt,
      Ipcns = ipcns,
      LabelNested = labelNested,
      LogConfiguration = logConfiguration,
      ManagePassword = managePassword,
      Mask = mask,
      NetworkOptions = networkOptions,
      Networks = networks,
      OomScoreAdj = oomScoreAdj,
      OverlayVolumes = overlayVolumes,
      PasswdEntry = passwdEntry,
      Personality = personality,
      Pidns = pidns,
      RawImageName = rawImageName,
      ReadWriteTmpfs = readWriteTmpfs,
      ResourceLimits = resourceLimits,
      RestartTries = restartTries,
      Rootfs = rootfs,
      RootfsMapping = rootfsMapping,
      RootfsOverlay = rootfsOverlay,
      RootfsPropagation = rootfsPropagation,
      SdnotifyMode = sdnotifyMode,
      Secrets = secrets,
      SelinuxOpts = selinuxOpts,
      ShmSize = shmSize,
      ShmSizeSystemd = shmSizeSystemd,
      StartupHealthConfig = startupHealthConfig,
      StopSignal = stopSignal,
      Systemd = systemd,
      ThrottleReadBpsDevice = throttleReadBpsDevice,
      ThrottleReadIopsDevice = throttleReadIopsDevice,
      ThrottleWriteBpsDevice = throttleWriteBpsDevice,
      ThrottleWriteIopsDevice = throttleWriteIopsDevice,
      Timeout = timeout,
      Umask = umask,
      Unified = unified,
      UseImageHosts = useImageHosts,
      UseImageResolvConf = useImageResolvConf,
      User = user,
      Userns = userns,
      Utsns = utsns,
      Volatile = volatileFlag,
      Volumes = volumes,
      VolumesFrom = volumesFrom,
      WeightDevice = weightDevice
    };

    return await PostJsonAsync<CreateContainerRequest, CreateContainerResponseDto>(
      "/libpod/containers/create",
      "Create container",
      createContainerParameters,
      PodmanJsonContext.Default.CreateContainerRequest,
      PodmanJsonContext.Default.CreateContainerResponseDto
    ).ConfigureAwait(false);
  }

  public async Task<Result> StartContainerAsync(string containerId, string detachKeys = "ctrl-p,ctrl-q") {
    var response = await _httpClient.PostAsync(
      $"/{_apiVersion}/libpod/containers/{containerId}/start?detachKeys={Uri.EscapeDataString(detachKeys)}",
      null
    );

    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified) {
      var message = response.StatusCode == HttpStatusCode.NotModified
        ? "Container was already started."
        : "Container started successfully.";

      if (response.StatusCode == HttpStatusCode.NotModified)
        _logger.LogWarning(message);
      else
        _logger.LogInformation(message);

      return PodmanHttpResults.Success(response.StatusCode, message);
    }

    var body = await response.Content.ReadAsStringAsync();
    var errorMessage = PodmanHttpResults.GetErrorMessage(body);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, "Start container", errorMessage);
    return PodmanHttpResults.Failure(response.StatusCode, errorMessage);
  }

  public async Task<Result> StopContainerAsync(string containerId, int timeout = 10, bool ignoreAlreadyStopped = false) {
    var queryParams = $"?timeout={timeout}&Ignore={ignoreAlreadyStopped.ToString().ToLower()}";
    var response = await _httpClient.PostAsync($"/{_apiVersion}/libpod/containers/{containerId}/stop{queryParams}", null);

    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified) {
      var message = response.StatusCode == HttpStatusCode.NotModified
        ? "Container was already stopped."
        : "Container stopped successfully.";

      if (response.StatusCode == HttpStatusCode.NotModified)
        _logger.LogWarning(message);
      else
        _logger.LogInformation(message);

      return PodmanHttpResults.Success(response.StatusCode, message);
    }

    var body = await response.Content.ReadAsStringAsync();
    var errorMessage = PodmanHttpResults.GetErrorMessage(body);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, "Stop container", errorMessage);
    return PodmanHttpResults.Failure(response.StatusCode, errorMessage);
  }

  public async Task<Result<DeleteContainerResponseDto[]?>> ForceDeleteContainerAsync(
    string containerId,
    bool deleteVolumes = false,
    int timeout = 10
  ) {
    var queryParams = $"?force=true&v={deleteVolumes.ToString().ToLower()}&timeout={timeout}";
    var response = await _httpClient.DeleteAsync($"/{_apiVersion}/libpod/containers/{containerId}{queryParams}");
    var jsonResponse = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode) {
      var value = !string.IsNullOrWhiteSpace(jsonResponse)
        ? JsonSerializer.Deserialize(jsonResponse, PodmanJsonContext.Default.DeleteContainerResponseDtoArray)
        : null;
      return PodmanHttpResults.Success(response.StatusCode, value);
    }

    var message = PodmanHttpResults.GetErrorMessage(jsonResponse);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, "Force delete container", message);
    return PodmanHttpResults.Failure<DeleteContainerResponseDto[]>(response.StatusCode, message);
  }

  public async Task<Result<DeleteContainerResponseDto[]?>> DeleteContainerAsync(
    string containerId,
    bool depend = false,
    bool ignore = false,
    int timeout = 10
  ) {
    var queryParams = $"?depend={depend.ToString().ToLower()}&ignore={ignore.ToString().ToLower()}&timeout={timeout}";
    var response = await _httpClient.DeleteAsync($"/{_apiVersion}/libpod/containers/{containerId}{queryParams}");
    var jsonResponse = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode) {
      var value = !string.IsNullOrWhiteSpace(jsonResponse)
        ? JsonSerializer.Deserialize(jsonResponse, PodmanJsonContext.Default.DeleteContainerResponseDtoArray)
        : null;
      return PodmanHttpResults.Success(response.StatusCode, value);
    }

    var message = PodmanHttpResults.GetErrorMessage(jsonResponse);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, "Delete container", message);
    return PodmanHttpResults.Failure<DeleteContainerResponseDto[]>(response.StatusCode, message);
  }


  public async Task<Result> ExtractArchiveToContainerAsync(string containerId, Stream tarStream, string path, bool pause = true) {
    var content = new StreamContent(tarStream);
    content.Headers.Add("Content-Type", "application/x-tar");

    var queryParams = $"?path={Uri.EscapeDataString(path)}&pause={pause.ToString().ToLower()}";
    var response = await _httpClient.PutAsync($"/{_apiVersion}/libpod/containers/{containerId}/archive{queryParams}", content);
    var body = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode) {
      _logger.LogInformation("Files copied successfully to the container. {Response}", body.Trim());
      return PodmanHttpResults.Success(response.StatusCode, "Files copied successfully to the container.");
    }

    var message = PodmanHttpResults.GetErrorMessage(body);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, "Copy files to container", message);
    return PodmanHttpResults.Failure(response.StatusCode, message);
  }
}
