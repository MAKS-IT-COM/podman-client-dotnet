using System.Management.Automation;

namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommunications.Connect, "Podman")]
[OutputType(typeof(void))]
public sealed class ConnectPodmanCmdlet : PSCmdlet {
  [Parameter(Mandatory = true, Position = 0)]
  public string BaseAddress { get; set; } = null!;

  [Parameter]
  public int TimeoutMinutes { get; set; } = 60;

  [Parameter]
  public string? ApiVersion { get; set; }

  protected override void ProcessRecord() {
    PodmanConnectionState.SetConnection(BaseAddress, TimeoutMinutes, ApiVersion);
    WriteVerbose($"Connected to Podman at {BaseAddress}");
  }
}

[Cmdlet(VerbsCommunications.Disconnect, "Podman")]
[OutputType(typeof(void))]
public sealed class DisconnectPodmanCmdlet : PSCmdlet {
  protected override void ProcessRecord() {
    PodmanConnectionState.ClearConnection();
    WriteVerbose("Disconnected from Podman");
  }
}
