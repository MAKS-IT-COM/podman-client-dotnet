using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Network;
using MaksIT.PodmanClientDotNet.Models.Network;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommon.New, "PodmanNetwork")]
[OutputType(typeof(NetworkListEntryDto))]
public sealed class NewPodmanNetworkCmdlet : PodmanCmdletBase {
  [Parameter(Position = 0)]
  public string? Name { get; set; }

  [Parameter]
  public NetworkCreateRequest? Request { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var request = Request ?? new NetworkCreateRequest();
      if (!string.IsNullOrWhiteSpace(Name))
        request.Name = Name;

      var result = client.CreateNetworkAsync(request).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanNetworkList")]
[OutputType(typeof(NetworkListEntryDto))]
public sealed class GetPodmanNetworkListCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ListNetworksAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanNetwork")]
[OutputType(typeof(NetworkInspectDto))]
public sealed class GetPodmanNetworkCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectNetworkAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanNetwork", SupportsShouldProcess = true)]
[OutputType(typeof(void))]
public sealed class RemovePodmanNetworkCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      if (!ShouldProcess(Name, "Remove Podman network"))
        return;

      var client = RequireClient();
      var result = client.DeleteNetworkAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommunications.Connect, "PodmanNetwork")]
[OutputType(typeof(void))]
public sealed class ConnectPodmanNetworkCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter(Position = 1)]
  public string? Container { get; set; }

  [Parameter]
  public NetworkConnectRequest? Request { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var request = Request ?? new NetworkConnectRequest();
      if (!string.IsNullOrWhiteSpace(Container))
        request.Container = Container;

      var result = client.ConnectNetworkAsync(Name, request).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommunications.Disconnect, "PodmanNetwork")]
[OutputType(typeof(void))]
public sealed class DisconnectPodmanNetworkCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter(Position = 1)]
  public string? Container { get; set; }

  [Parameter]
  public SwitchParameter Force { get; set; }

  [Parameter]
  public NetworkDisconnectRequest? Request { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var request = Request ?? new NetworkDisconnectRequest();
      if (!string.IsNullOrWhiteSpace(Container))
        request.Container = Container;
      if (Force.IsPresent)
        request.Force = true;

      var result = client.DisconnectNetworkAsync(Name, request).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
