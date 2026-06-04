using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Container;
using MaksIT.PodmanClientDotNet.Models;
using MaksIT.PodmanClientDotNet.Models.Container;
using MaksIT.PodmanClientDotNet.Streaming;
using MaksIT.Results;

/// <summary>
/// Container lifecycle, inspection, attach, logs, archive, and related libpod endpoints.
/// </summary>
public interface IPodmanContainersClient {
  Task<Result<CreateContainerResponseDto?>> CreateContainerAsync(
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
  );

  Task<Result> StartContainerAsync(string containerId, string detachKeys = "ctrl-p,ctrl-q");
  Task<Result> StopContainerAsync(string containerId, int timeout = 10, bool ignoreAlreadyStopped = false);
  Task<Result<DeleteContainerResponseDto[]?>> ForceDeleteContainerAsync(string containerId, bool deleteVolumes = false, int timeout = 10);
  Task<Result<DeleteContainerResponseDto[]?>> DeleteContainerAsync(string containerId, bool depend = false, bool ignore = false, int timeout = 10);
  Task<Result> ExtractArchiveToContainerAsync(string containerId, Stream tarStream, string path, bool pause = true);

  Task<Result<List<ContainerListEntryDto>?>> ListContainersAsync(
    bool all = false,
    int? limit = null,
    bool size = false,
    bool sync = false,
    string? filters = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<ContainerInspectDto?>> InspectContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> ContainerExistsAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> RestartContainerAsync(string name, int timeout = 10, CancellationToken cancellationToken = default);
  Task<Result> KillContainerAsync(string name, string signal = "TERM", CancellationToken cancellationToken = default);
  Task<Result> PauseContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> UnpauseContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<ContainerWaitDto?>> WaitContainerAsync(string name, string? condition = null, CancellationToken cancellationToken = default);
  Task<Result<Stream?>> GetContainerLogsAsync(
    string name,
    bool follow = false,
    bool stdout = true,
    bool stderr = true,
    bool timestamps = false,
    string? since = null,
    string? until = null,
    string? tail = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<ContainerStatsDto?>> GetContainerStatsAsync(string name, bool stream = false, CancellationToken cancellationToken = default);
  Task<Result<Dictionary<string, ContainerStatsDto>?>> GetContainersStatsAsync(
    IEnumerable<string>? containers = null,
    bool stream = false,
    CancellationToken cancellationToken = default
  );

  Task<Result<PruneReportDto?>> PruneContainersAsync(string? filters = null, CancellationToken cancellationToken = default);
  Task<Result> RenameContainerAsync(string name, string newName, CancellationToken cancellationToken = default);
  Task<Result> InitContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<Stream?>> CheckpointContainerAsync(
    string name,
    bool keep = false,
    bool leaveRunning = false,
    bool tcpEstablished = false,
    bool export = false,
    bool ignoreRootFS = false,
    CancellationToken cancellationToken = default
  );

  Task<Result> RestoreContainerAsync(
    string name,
    string? importPath = null,
    bool keep = false,
    bool leaveRunning = false,
    bool tcpEstablished = false,
    bool ignoreRootFS = false,
    bool ignoreStaticIP = false,
    bool ignoreStaticMAC = false,
    CancellationToken cancellationToken = default
  );

  Task<Result<ContainerMountDto?>> MountContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result> UnmountContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<Stream?>> ExportContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<Stream?>> GetContainerArchiveAsync(string name, string path, CancellationToken cancellationToken = default);
  Task<Result> PutContainerArchiveAsync(string containerId, Stream tarStream, string path, bool pause = true, CancellationToken cancellationToken = default);
  Task<Result<Stream?>> AttachContainerAsync(
    string name,
    bool logs = false,
    bool stream = true,
    bool stdout = true,
    bool stderr = true,
    bool stdin = false,
    string? detachKeys = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Opens a full-duplex attach session (multiplexed or raw TTY) over a hijacked HTTP connection.
  /// </summary>
  Task<Result<IPodmanAttachSession?>> AttachContainerSessionAsync(
    string name,
    bool logs = false,
    bool stream = true,
    bool stdout = true,
    bool stderr = true,
    bool stdin = true,
    bool tty = false,
    string? detachKeys = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<ContainerChangesDto?>> GetContainerChangesAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<ContainerCommitDto?>> CommitContainerAsync(
    string container,
    string? repo = null,
    string? tag = null,
    string? comment = null,
    string? author = null,
    bool pause = true,
    IEnumerable<string>? changes = null,
    string? format = null,
    CancellationToken cancellationToken = default
  );

  Task<Result<ContainerHealthCheckDto?>> HealthCheckContainerAsync(string name, CancellationToken cancellationToken = default);
  Task<Result<MountedContainersResponseDto?>> GetMountedContainersAsync(CancellationToken cancellationToken = default);
  Task<Result<ContainerTopDto?>> TopContainerAsync(string name, string psArgs = "-ef", bool stream = true, CancellationToken cancellationToken = default);
}
