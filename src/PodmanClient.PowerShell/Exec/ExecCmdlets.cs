using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Exec;
using MaksIT.PodmanClientDotNet.Streaming;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommon.New, "PodmanExec")]
[OutputType(typeof(CreateExecResponseDto))]
public sealed class NewPodmanExecCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string ContainerName { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string[] Cmd { get; set; } = null!;

  [Parameter]
  public bool AttachStderr { get; set; } = true;

  [Parameter]
  public SwitchParameter AttachStdin { get; set; }

  [Parameter]
  public bool AttachStdout { get; set; } = true;

  [Parameter]
  public string? DetachKeys { get; set; }

  [Parameter]
  public string[]? Env { get; set; }

  [Parameter]
  public SwitchParameter Privileged { get; set; }

  [Parameter]
  public SwitchParameter Tty { get; set; }

  [Parameter]
  public string? User { get; set; }

  [Parameter]
  public string? WorkingDir { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.CreateExecAsync(
        ContainerName,
        Cmd,
        AttachStderr,
        AttachStdin.IsPresent,
        AttachStdout,
        DetachKeys,
        Env,
        Privileged.IsPresent,
        Tty.IsPresent,
        User,
        WorkingDir).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Start, "PodmanExec")]
public sealed class StartPodmanExecCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string ExecId { get; set; } = null!;

  [Parameter]
  public SwitchParameter Detach { get; set; }

  [Parameter]
  public SwitchParameter Tty { get; set; }

  [Parameter]
  public int? Height { get; set; }

  [Parameter]
  public int? Width { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.StartExecAsync(ExecId, Detach.IsPresent, Tty.IsPresent, Height, Width)
        .GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanExec")]
[OutputType(typeof(InspectExecResponseDto))]
public sealed class GetPodmanExecCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string ExecId { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectExecAsync(ExecId).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Resize, "PodmanExec")]
public sealed class ResizePodmanExecCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string ExecId { get; set; } = null!;

  [Parameter(Mandatory = true)]
  public int Height { get; set; }

  [Parameter(Mandatory = true)]
  public int Width { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ResizeExecAsync(ExecId, Height, Width).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanExecSession")]
[OutputType(typeof(string))]
[OutputType(typeof(IPodmanAttachSession))]
public sealed class InvokePodmanExecSessionCmdlet : PodmanCmdletBase {
  public InvokePodmanExecSessionCmdlet() {
    CollectOutput = true;
  }

  [Parameter(Mandatory = true, Position = 0)]
  public string ExecId { get; set; } = null!;

  [Parameter]
  public SwitchParameter Tty { get; set; }

  [Parameter]
  public int? Height { get; set; }

  [Parameter]
  public int? Width { get; set; }

  [Parameter]
  public SwitchParameter CollectOutput { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.StartExecSessionAsync(ExecId, Tty.IsPresent, Height, Width)
        .GetAwaiter().GetResult();

      if (!CollectOutput) {
        WritePodmanResult(result);
        return;
      }

      if (!result.IsSuccess) {
        WritePodmanResult(result);
        return;
      }

      if (result.Value is null)
        return;

      try {
        WriteObject(CollectAttachOutput(result.Value));
      }
      finally {
        result.Value.DisposeAsync().AsTask().GetAwaiter().GetResult();
      }
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
