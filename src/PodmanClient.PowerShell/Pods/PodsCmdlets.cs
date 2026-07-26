using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Pod;
using MaksIT.PodmanClientDotNet.Models.Pod;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommon.New, "PodmanPod")]
[OutputType(typeof(PodListEntryDto))]
public sealed class NewPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Position = 0)]
  public string? Name { get; set; }

  [Parameter]
  public PodCreateRequest? Request { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var request = Request ?? new PodCreateRequest();
      if (!string.IsNullOrWhiteSpace(Name))
        request.Name = Name;

      var result = client.CreatePodAsync(request).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanPodList")]
[OutputType(typeof(PodListEntryDto))]
public sealed class GetPodmanPodListCmdlet : PodmanCmdletBase {
  [Parameter]
  public SwitchParameter All { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ListPodsAsync(All.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanPod")]
[OutputType(typeof(PodInspectDto))]
public sealed class GetPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectPodAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsDiagnostic.Test, "PodmanPod")]
[OutputType(typeof(bool))]
public sealed class TestPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PodExistsAsync(Name).GetAwaiter().GetResult();
      WriteObject(result.IsSuccess);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanPod", SupportsShouldProcess = true)]
[OutputType(typeof(void))]
public sealed class RemovePodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Force { get; set; }

  protected override void ProcessRecord() {
    try {
      if (!ShouldProcess(Name, "Remove Podman pod"))
        return;

      var client = RequireClient();
      var result = client.DeletePodAsync(Name, Force.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Start, "PodmanPod")]
[OutputType(typeof(void))]
public sealed class StartPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.StartPodAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Stop, "PodmanPod")]
[OutputType(typeof(void))]
public sealed class StopPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public int Timeout { get; set; } = 10;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.StopPodAsync(Name, Timeout).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Restart, "PodmanPod")]
[OutputType(typeof(void))]
public sealed class RestartPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public int Timeout { get; set; } = 10;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.RestartPodAsync(Name, Timeout).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet("Kill", "PodmanPod")]
[OutputType(typeof(void))]
public sealed class KillPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Signal { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.KillPodAsync(Name, Signal).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Suspend, "PodmanPod")]
[OutputType(typeof(void))]
public sealed class SuspendPodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PausePodAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Resume, "PodmanPod")]
[OutputType(typeof(void))]
public sealed class ResumePodmanPodCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.UnpausePodAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPrunePod")]
[OutputType(typeof(PruneReportEntryDto))]
public sealed class InvokePodmanPrunePodCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PrunePodsAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanPodTop")]
[OutputType(typeof(PodTopDto))]
public sealed class GetPodmanPodTopCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.TopPodAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanPodStat")]
[OutputType(typeof(PodStatsDto))]
public sealed class GetPodmanPodStatCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetPodsStatsAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
