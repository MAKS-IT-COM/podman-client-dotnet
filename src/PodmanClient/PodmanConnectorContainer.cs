using System.Text;
using System.Text.Json;

using MaksIT.PodmanClientDotNet.Extensions;

using MaksIT.PodmanClientDotNet.Models;
using MaksIT.PodmanClientDotNet.Models.Container;

namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {
    public async Task<CreateContainerResponse> CreateContainerAsync(
      string name,
      string image,
      List<string> command = null,
      Dictionary<string, string> env = null,
      bool? remove = null,
      bool? stdin = null,
      bool? terminal = null,
      List<Mount> mounts = null,
      bool? privileged = null,
      string hostname = null,
      Namespace netns = null,
      List<PortMapping> portMappings = null,
      string restartPolicy = null,
      ulong? stopTimeout = null,
      List<string> capAdd = null,
      List<string> capDrop = null,
      List<string> dnsServers = null,
      List<string> dnsSearch = null,
      List<string> dnsOptions = null,
      bool? publishImagePorts = null,
      List<string> cniNetworks = null,
      Dictionary<string, string> labels = null,
      bool? readOnlyFilesystem = null,
      List<POSIXRlimit> rLimits = null,
      List<LinuxDevice> devices = null,
      string ociRuntime = null,
      string pod = null,
      bool? noNewPrivileges = null,
      string cgroupsMode = null,
      Dictionary<string, string> storageOpts = null,
      bool? unsetenvall = null,
      Dictionary<string, string> secretEnv = null,
      string timezone = null,
      Dictionary<string, string> sysctl = null,
      string seccompProfilePath = null,
      string seccompPolicy = null,
      Dictionary<string, string> annotations = null,
      string apparmorProfile = null,
      string baseHostsFile = null,
      string cgroupParent = null,
      Namespace cgroupns = null,
      List<string> chrootDirectories = null,
      string conmonPidFile = null,
      List<string> containerCreateCommand = null,
      bool? createWorkingDir = null,
      List<string> dependencyContainers = null,
      List<LinuxDeviceCgroup> deviceCgroupRule = null,
      List<string> devicesFrom = null,
      List<string> entrypoint = null,
      bool? envHost = null,
      List<string> envMerge = null,
      Dictionary<ushort, string> expose = null,
      string groupEntry = null,
      List<string> groups = null,
      long? healthCheckOnFailureAction = null,
      Schema2HealthConfig healthConfig = null,
      List<LinuxDevice> hostDeviceList = null,
      List<string> hostAdd = null,
      List<string> hostUsers = null,
      bool? envHTTPProxy = null,
      IDMappingOptions idMappings = null,
      string imageArch = null,
      string imageOS = null,
      string imageVariant = null,
      string imageVolumeMode = null,
      List<ImageVolume> imageVolumes = null,
      bool? init = null,
      string initContainerType = null,
      string initPath = null,
      LinuxIntelRdt intelRdt = null,
      Namespace ipcns = null,
      bool? labelNested = null,
      LogConfigLibpod logConfiguration = null,
      bool? managePassword = null,
      List<string> mask = null,
      Dictionary<string, string> networkOptions = null,
      Dictionary<string, NetworkSettings> networks = null,
      long? oomScoreAdj = null,
      List<OverlayVolume> overlayVolumes = null,
      string passwdEntry = null,
      LinuxPersonality personality = null,
      Namespace pidns = null,
      string rawImageName = null,
      bool? readWriteTmpfs = null,
      LinuxResources resourceLimits = null,
      ulong? restartTries = null,
      string rootfs = null,
      string rootfsMapping = null,
      bool? rootfsOverlay = null,
      string rootfsPropagation = null,
      string sdnotifyMode = null,
      List<SecretProp> secrets = null,
      List<string> selinuxOpts = null,
      long? shmSize = null,
      long? shmSizeSystemd = null,
      StartupHealthConfig startupHealthConfig = null,
      long? stopSignal = null,
      string systemd = null,
      Dictionary<string, ulong> throttleReadBpsDevice = null,
      Dictionary<string, ulong> throttleReadIopsDevice = null,
      Dictionary<string, ulong> throttleWriteBpsDevice = null,
      Dictionary<string, ulong> throttleWriteIopsDevice = null,
      ulong? timeout = null,
      string umask = null,
      Dictionary<string, string> unified = null,
      List<string> unmask = null,
      bool? useImageHosts = null,
      bool? useImageResolvConf = null,
      string user = null,
      Namespace userns = null,
      Namespace utsns = null,
      bool? volatileFlag = null,
      List<NamedVolume> volumes = null,
      List<string> volumesFrom = null,
      Dictionary<string, ulong> weightDevice = null,
      string workDir = null
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

      var jsonContent = new StringContent(JsonSerializer.Serialize(createContainerParameters), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync("/v1.41/libpod/containers/create", jsonContent);

      if (response.IsSuccessStatusCode) {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreateContainerResponse>(jsonResponse);
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            Console.WriteLine($"Bad parameter in request: {errorDetails?.Message}");
            break;
          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such container: {errorDetails?.Message}");
            break;
          case System.Net.HttpStatusCode.Conflict:
            Console.WriteLine($"Conflict error in operation: {errorDetails?.Message}");
            break;
          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;
          default:
            Console.WriteLine($"Error creating container: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();

        return null;
      }
    }



    public async Task StartContainerAsync(string containerId, string detachKeys = "ctrl-p,ctrl-q") {
      var response = await _httpClient.PostAsync(
          $"/v1.41/libpod/containers/{containerId}/start?detachKeys={Uri.EscapeDataString(detachKeys)}", null);

      switch (response.StatusCode) {
        case System.Net.HttpStatusCode.NoContent:
          Console.WriteLine("Container started successfully.");
          break;

        case System.Net.HttpStatusCode.NotModified:
          Console.WriteLine("Container was already started.");
          break;

        case System.Net.HttpStatusCode.NotFound:
          var errorContent404 = await response.Content.ReadAsStringAsync();
          var errorDetails404 = errorContent404.ToObject<ErrorResponse>();
          Console.WriteLine($"Container not found: {errorDetails404?.Message}");
          break;

        case System.Net.HttpStatusCode.InternalServerError:
          var errorContent500 = await response.Content.ReadAsStringAsync();
          var errorDetails500 = errorContent500.ToObject<ErrorResponse>();
          Console.WriteLine($"Internal server error: {errorDetails500?.Message}");
          break;

        default:
          if ((int)response.StatusCode >= 400) {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorDetails = errorContent.ToObject<ErrorResponse>();
            Console.WriteLine($"Error starting container: {errorDetails?.Message}");
          }
          break;
      }

      response.EnsureSuccessStatusCode();
    }

    public async Task StopContainerAsync(string containerId, int timeout = 10, bool ignoreAlreadyStopped = false) {
      var queryParams = $"?timeout={timeout}&Ignore={ignoreAlreadyStopped.ToString().ToLower()}";
      var response = await _httpClient.PostAsync($"/v1.41/libpod/containers/{containerId}/stop{queryParams}", null);

      if (response.IsSuccessStatusCode) {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent) {
          Console.WriteLine("Container stopped successfully.");
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotModified) {
          Console.WriteLine("Container was already stopped.");
        }
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such container: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            Console.WriteLine($"Error stopping container: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
      }
    }

    public async Task ForceDeleteContainerAsync(string containerId, bool deleteVolumes = false, int timeout = 10) {
      var queryParams = $"?force=true&v={deleteVolumes.ToString().ToLower()}&timeout={timeout}";
      var response = await _httpClient.DeleteAsync($"/v1.41/libpod/containers/{containerId}{queryParams}");

      if (response.IsSuccessStatusCode) {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent) {
          Console.WriteLine("Container force deleted successfully.");
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.OK) {
          var responseContent = await response.Content.ReadAsStringAsync();
          var deleteResponses = JsonSerializer.Deserialize<DeleteContainerResponse[]>(responseContent);

          foreach (var deleteResponse in deleteResponses) {
            if (string.IsNullOrEmpty(deleteResponse.Err)) {
              Console.WriteLine($"Container {deleteResponse.Id} deleted successfully.");
            }
            else {
              Console.WriteLine($"Error deleting container {deleteResponse.Id}: {deleteResponse.Err}");
            }
          }
        }
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            Console.WriteLine($"Bad parameter in request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such container: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.Conflict:
            Console.WriteLine($"Conflict error: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            Console.WriteLine($"Error deleting container: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
      }
    }

    public async Task DeleteContainerAsync(string containerId, bool depend = false, bool ignore = false, int timeout = 10) {
      var queryParams = $"?depend={depend.ToString().ToLower()}&ignore={ignore.ToString().ToLower()}&timeout={timeout}";
      var response = await _httpClient.DeleteAsync($"/v1.41/libpod/containers/{containerId}{queryParams}");

      if (response.IsSuccessStatusCode) {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent) {
          Console.WriteLine("Container deleted successfully.");
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.OK) {
          var responseContent = await response.Content.ReadAsStringAsync();
          var deleteResponses = JsonSerializer.Deserialize<DeleteContainerResponse[]>(responseContent);

          foreach (var deleteResponse in deleteResponses) {
            if (string.IsNullOrEmpty(deleteResponse.Err)) {
              Console.WriteLine($"Container {deleteResponse.Id} deleted successfully.");
            }
            else {
              Console.WriteLine($"Error deleting container {deleteResponse.Id}: {deleteResponse.Err}");
            }
          }
        }
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            Console.WriteLine($"Bad parameter in request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such container: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.Conflict:
            Console.WriteLine($"Conflict error: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            Console.WriteLine($"Error deleting container: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode(); // Throws an exception if the response indicates an error
      }

    }


    public async Task ExtractArchiveToContainerAsync(string containerId, Stream tarStream, string path, bool pause = true) {
      var content = new StreamContent(tarStream);
      content.Headers.Add("Content-Type", "application/x-tar");

      var queryParams = $"?path={Uri.EscapeDataString(path)}&pause={pause.ToString().ToLower()}";
      var response = await _httpClient.PutAsync($"/v1.41/libpod/containers/{containerId}/archive{queryParams}", content);

      if (response.IsSuccessStatusCode) {
        Console.WriteLine("Files copied successfully to the container.");
      }
      else {
        var errorContent = await response.Content.ReadAsStringAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

        switch (response.StatusCode) {
          case System.Net.HttpStatusCode.BadRequest:
            Console.WriteLine($"Bad parameter in request: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.Forbidden:
            Console.WriteLine($"The container root filesystem is read-only: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.NotFound:
            Console.WriteLine($"No such container: {errorDetails?.Message}");
            break;

          case System.Net.HttpStatusCode.InternalServerError:
            Console.WriteLine($"Internal server error: {errorDetails?.Message}");
            break;

          default:
            Console.WriteLine($"Error copying files: {errorDetails?.Message}");
            break;
        }

        response.EnsureSuccessStatusCode();
      }
    }

  }
}
