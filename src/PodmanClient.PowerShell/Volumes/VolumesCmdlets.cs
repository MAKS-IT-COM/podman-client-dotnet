using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Volume;
using MaksIT.PodmanClientDotNet.Models.Volume;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommon.New, "PodmanVolume")]
[OutputType(typeof(VolumeInspectResponseDto))]
public sealed class NewPodmanVolumeCmdlet : PodmanCmdletBase {
  [Parameter(Position = 0)]
  public string? Name { get; set; }

  [Parameter]
  public string? Driver { get; set; }

  [Parameter]
  public CreateVolumeRequest? Request { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var request = Request ?? new CreateVolumeRequest {
        Name = Name,
        Driver = Driver,
      };
      var result = client.CreateVolumeAsync(request).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanVolumeList")]
[OutputType(typeof(VolumeListEntryDto))]
public sealed class GetPodmanVolumeListCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ListVolumesAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanVolume")]
[OutputType(typeof(VolumeInspectResponseDto))]
public sealed class GetPodmanVolumeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectVolumeAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanVolume", SupportsShouldProcess = true)]
[OutputType(typeof(void))]
public sealed class RemovePodmanVolumeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Force { get; set; }

  protected override void ProcessRecord() {
    try {
      if (!ShouldProcess(Name, "Remove Podman volume"))
        return;

      var client = RequireClient();
      var result = client.DeleteVolumeAsync(Name, Force.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPruneVolume")]
[OutputType(typeof(PruneReportEntryDto))]
public sealed class InvokePodmanPruneVolumeCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PruneVolumesAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
