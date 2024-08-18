
namespace MaksIT.PodmanClientDotNet.Models.Container {
  public class CreateContainerRequest {
    public Dictionary<string, string> Annotations { get; set; }
    public string ApparmorProfile { get; set; }
    public string BaseHostsFile { get; set; }
    public List<string> CapAdd { get; set; }
    public List<string> CapDrop { get; set; }
    public string CgroupParent { get; set; }
    public Namespace Cgroupns { get; set; }
    public string CgroupsMode { get; set; }
    public List<string> ChrootDirectories { get; set; }
    public List<string> CNINetworks { get; set; }
    public List<string> Command { get; set; }
    public string ConmonPidFile { get; set; }
    public List<string> ContainerCreateCommand { get; set; }
    public bool? CreateWorkingDir { get; set; }
    public List<string> DependencyContainers { get; set; }
    public List<LinuxDeviceCgroup> DeviceCgroupRule { get; set; }
    public List<LinuxDevice> Devices { get; set; }
    public List<string> DevicesFrom { get; set; }
    public List<string> DNSOption { get; set; }
    public List<string> DNSSearch { get; set; }
    public List<string> DNSServer { get; set; }
    public List<string> Entrypoint { get; set; }
    public Dictionary<string, string> Env { get; set; }
    public bool? EnvHost { get; set; }
    public List<string> EnvMerge { get; set; }
    public Dictionary<ushort, string> Expose { get; set; }
    public string GroupEntry { get; set; }
    public List<string> Groups { get; set; }
    public long? HealthCheckOnFailureAction { get; set; }
    public Schema2HealthConfig HealthConfig { get; set; }
    public List<LinuxDevice> HostDeviceList { get; set; }
    public List<string> HostAdd { get; set; }
    public string Hostname { get; set; }
    public List<string> HostUsers { get; set; }
    public bool? EnvHTTPProxy { get; set; }
    public IDMappingOptions IDMappings { get; set; }
    public string Image { get; set; }
    public string ImageArch { get; set; }
    public string ImageOS { get; set; }
    public string ImageVariant { get; set; }
    public string ImageVolumeMode { get; set; }
    public List<ImageVolume> ImageVolumes { get; set; }
    public bool? Init { get; set; }
    public string InitContainerType { get; set; }
    public string InitPath { get; set; }
    public LinuxIntelRdt IntelRdt { get; set; }
    public Namespace Ipcns { get; set; }
    public bool? LabelNested { get; set; }
    public Dictionary<string, string> Labels { get; set; }
    public LogConfigLibpod LogConfiguration { get; set; }
    public bool? ManagePassword { get; set; }
    public List<string> Mask { get; set; }
    public List<Mount> Mounts { get; set; }
    public string Name { get; set; }
    public Namespace Netns { get; set; }
    public Dictionary<string, string> NetworkOptions { get; set; }
    public Dictionary<string, NetworkSettings> Networks { get; set; }
    public bool? NoNewPrivileges { get; set; }
    public string OciRuntime { get; set; }
    public long? OomScoreAdj { get; set; }
    public List<OverlayVolume> OverlayVolumes { get; set; }
    public string PasswdEntry { get; set; }
    public LinuxPersonality Personality { get; set; }
    public Namespace Pidns { get; set; }
    public string Pod { get; set; }
    public List<PortMapping> Portmappings { get; set; }
    public bool? Privileged { get; set; }
    public List<string> ProcfsOpts { get; set; }
    public bool? PublishImagePorts { get; set; }
    public List<POSIXRlimit> RLimits { get; set; }
    public string RawImageName { get; set; }
    public bool? ReadOnlyFilesystem { get; set; }
    public bool? ReadWriteTmpfs { get; set; }
    public bool? Remove { get; set; }
    public LinuxResources ResourceLimits { get; set; }
    public string RestartPolicy { get; set; }
    public ulong? RestartTries { get; set; }
    public string Rootfs { get; set; }
    public string RootfsMapping { get; set; }
    public bool? RootfsOverlay { get; set; }
    public string RootfsPropagation { get; set; }
    public string SdnotifyMode { get; set; }
    public string SeccompPolicy { get; set; }
    public string SeccompProfilePath { get; set; }
    public Dictionary<string, string> SecretEnv { get; set; }
    public List<SecretProp> Secrets { get; set; }
    public List<string> SelinuxOpts { get; set; }
    public long? ShmSize { get; set; }
    public long? ShmSizeSystemd { get; set; }
    public StartupHealthConfig StartupHealthConfig { get; set; }
    public bool? Stdin { get; set; }
    public long? StopSignal { get; set; }
    public ulong? StopTimeout { get; set; }
    public Dictionary<string, string> StorageOpts { get; set; }
    public Dictionary<string, string> Sysctl { get; set; }
    public string Systemd { get; set; }
    public bool? Terminal { get; set; }
    public Dictionary<string, ulong> ThrottleReadBpsDevice { get; set; }
    public Dictionary<string, ulong> ThrottleReadIopsDevice { get; set; }
    public Dictionary<string, ulong> ThrottleWriteBpsDevice { get; set; }
    public Dictionary<string, ulong> ThrottleWriteIopsDevice { get; set; }
    public ulong? Timeout { get; set; }
    public string Timezone { get; set; }
    public string Umask { get; set; }
    public Dictionary<string, string> Unified { get; set; }
    public List<string> Unmask { get; set; }
    public List<string> Unsetenv { get; set; }
    public bool? Unsetenvall { get; set; }
    public bool? UseImageHosts { get; set; }
    public bool? UseImageResolvConf { get; set; }
    public string User { get; set; }
    public Namespace Userns { get; set; }
    public Namespace Utsns { get; set; }
    public bool? Volatile { get; set; }
    public List<NamedVolume> Volumes { get; set; }
    public List<string> VolumesFrom { get; set; }
    public Dictionary<string, ulong> WeightDevice { get; set; }
    public string WorkDir { get; set; }
  }

}
