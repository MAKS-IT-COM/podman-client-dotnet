using System.Collections;
using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Container;
using MaksIT.PodmanClientDotNet.Streaming;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommon.New, "PodmanContainer")]
[OutputType(typeof(CreateContainerResponseDto))]
public sealed class NewPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Image { get; set; } = null!;

  [Parameter]
  public string[]? Command { get; set; }

  [Parameter]
  public Hashtable? Env { get; set; }

  [Parameter]
  public SwitchParameter Remove { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var command = Command is null ? null : new List<string>(Command);
      var env = ToStringDictionary(Env);
      bool? remove = Remove.IsPresent ? true : null;
      var result = client.CreateContainerAsync(Name, Image, command, env, remove).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }

  private static Dictionary<string, string>? ToStringDictionary(Hashtable? table) {
    if (table is null)
      return null;

    var dict = new Dictionary<string, string>(table.Count);
    foreach (DictionaryEntry entry in table)
      dict[entry.Key?.ToString() ?? string.Empty] = entry.Value?.ToString() ?? string.Empty;

    return dict;
  }
}

[Cmdlet(VerbsLifecycle.Start, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class StartPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string DetachKeys { get; set; } = "ctrl-p,ctrl-q";

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.StartContainerAsync(Name, DetachKeys).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Stop, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class StopPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public int Timeout { get; set; } = 10;

  [Parameter]
  public SwitchParameter IgnoreAlreadyStopped { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.StopContainerAsync(Name, Timeout, IgnoreAlreadyStopped).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanContainer")]
[OutputType(typeof(DeleteContainerResponseDto))]
public sealed class RemovePodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Force { get; set; }

  [Parameter]
  public SwitchParameter DeleteVolumes { get; set; }

  [Parameter]
  public SwitchParameter Depend { get; set; }

  [Parameter]
  public SwitchParameter Ignore { get; set; }

  [Parameter]
  public int Timeout { get; set; } = 10;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      if (Force) {
        var forceResult = client.ForceDeleteContainerAsync(Name, DeleteVolumes, Timeout).GetAwaiter().GetResult();
        WritePodmanResult(forceResult);
        return;
      }

      var result = client.DeleteContainerAsync(Name, Depend, Ignore, Timeout).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanExtractArchive")]
[OutputType(typeof(void))]
public sealed class InvokePodmanExtractArchiveCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  [Alias("Name", "Id")]
  public string ContainerId { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Path { get; set; } = null!;

  [Parameter(ParameterSetName = "FilePath")]
  public string? FilePath { get; set; }

  [Parameter(ParameterSetName = "InputStream")]
  public Stream? InputStream { get; set; }

  [Parameter]
  public SwitchParameter Pause { get; set; } = true;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var ownsStream = InputStream is null;
      var stream = OpenInputStream(FilePath, InputStream);
      try {
        var result = client.ExtractArchiveToContainerAsync(ContainerId, stream, Path, Pause).GetAwaiter().GetResult();
        WritePodmanResult(result);
      }
      finally {
        if (ownsStream)
          stream.Dispose();
      }
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerList")]
[OutputType(typeof(ContainerListEntryDto))]
public sealed class GetPodmanContainerListCmdlet : PodmanCmdletBase {
  [Parameter]
  public SwitchParameter All { get; set; }

  [Parameter]
  public int? Limit { get; set; }

  [Parameter]
  public SwitchParameter Size { get; set; }

  [Parameter]
  public SwitchParameter Sync { get; set; }

  [Parameter]
  public string? Filters { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ListContainersAsync(All, Limit, Size, Sync, Filters).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainer")]
[OutputType(typeof(ContainerInspectDto))]
public sealed class GetPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsDiagnostic.Test, "PodmanContainer")]
[OutputType(typeof(bool))]
public sealed class TestPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ContainerExistsAsync(Name).GetAwaiter().GetResult();
      WriteObject(result.IsSuccess);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Restart, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class RestartPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public int Timeout { get; set; } = 10;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.RestartContainerAsync(Name, Timeout).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet("Kill", "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class KillPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string Signal { get; set; } = "TERM";

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.KillContainerAsync(Name, Signal).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Suspend, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class SuspendPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PauseContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Resume, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class ResumePodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.UnpauseContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Wait, "PodmanContainer")]
[OutputType(typeof(ContainerWaitDto))]
public sealed class WaitPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Condition { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.WaitContainerAsync(Name, Condition).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerLog")]
[OutputType(typeof(Stream), typeof(string))]
public sealed class GetPodmanContainerLogCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Follow { get; set; }

  [Parameter]
  public SwitchParameter Stdout { get; set; } = true;

  [Parameter]
  public SwitchParameter Stderr { get; set; } = true;

  [Parameter]
  public SwitchParameter Timestamps { get; set; }

  [Parameter]
  public string? Since { get; set; }

  [Parameter]
  public string? Until { get; set; }

  [Parameter]
  public string? Tail { get; set; }

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetContainerLogsAsync(
        Name,
        Follow,
        Stdout,
        Stderr,
        Timestamps,
        Since,
        Until,
        Tail).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerStat")]
[OutputType(typeof(ContainerStatsDto))]
public sealed class GetPodmanContainerStatCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Stream { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetContainerStatsAsync(Name, Stream).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerStatBatch")]
[OutputType(typeof(ContainersStatsResponseDto))]
public sealed class GetPodmanContainerStatBatchCmdlet : PodmanCmdletBase {
  [Parameter]
  public string[]? Containers { get; set; }

  [Parameter]
  public SwitchParameter Stream { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetContainersStatsAsync(Containers, Stream).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPruneContainer")]
[OutputType(typeof(PruneReportEntryDto))]
public sealed class InvokePodmanPruneContainerCmdlet : PodmanCmdletBase {
  [Parameter]
  public string? Filters { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PruneContainersAsync(Filters).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Rename, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class RenamePodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string NewName { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.RenameContainerAsync(Name, NewName).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Initialize, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class InitializePodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InitContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet("Checkpoint", "PodmanContainer")]
[OutputType(typeof(Stream), typeof(string))]
public sealed class CheckpointPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Keep { get; set; }

  [Parameter]
  public SwitchParameter LeaveRunning { get; set; }

  [Parameter]
  public SwitchParameter TcpEstablished { get; set; }

  [Parameter]
  public SwitchParameter Export { get; set; }

  [Parameter]
  public SwitchParameter IgnoreRootFS { get; set; }

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.CheckpointContainerAsync(
        Name,
        Keep,
        LeaveRunning,
        TcpEstablished,
        Export,
        IgnoreRootFS).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Restore, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class RestorePodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? ImportPath { get; set; }

  [Parameter]
  public SwitchParameter Keep { get; set; }

  [Parameter]
  public SwitchParameter LeaveRunning { get; set; }

  [Parameter]
  public SwitchParameter TcpEstablished { get; set; }

  [Parameter]
  public SwitchParameter IgnoreRootFS { get; set; }

  [Parameter]
  public SwitchParameter IgnoreStaticIP { get; set; }

  [Parameter]
  public SwitchParameter IgnoreStaticMAC { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.RestoreContainerAsync(
        Name,
        ImportPath,
        Keep,
        LeaveRunning,
        TcpEstablished,
        IgnoreRootFS,
        IgnoreStaticIP,
        IgnoreStaticMAC).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Mount, "PodmanContainer")]
[OutputType(typeof(ContainerMountDto))]
public sealed class MountPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.MountContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Dismount, "PodmanContainer")]
[OutputType(typeof(void))]
public sealed class DismountPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.UnmountContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Export, "PodmanContainer")]
[OutputType(typeof(Stream), typeof(string))]
public sealed class ExportPodmanContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ExportContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerArchive")]
[OutputType(typeof(Stream), typeof(string))]
public sealed class GetPodmanContainerArchiveCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Path { get; set; } = null!;

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetContainerArchiveAsync(Name, Path).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Set, "PodmanContainerArchive")]
[OutputType(typeof(void))]
public sealed class SetPodmanContainerArchiveCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  [Alias("Name", "Id")]
  public string ContainerId { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Path { get; set; } = null!;

  [Parameter]
  public string? FilePath { get; set; }

  [Parameter]
  public Stream? InputStream { get; set; }

  [Parameter]
  public SwitchParameter Pause { get; set; } = true;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var ownsStream = InputStream is null;
      var stream = OpenInputStream(FilePath, InputStream);
      try {
        var result = client.PutContainerArchiveAsync(ContainerId, stream, Path, Pause).GetAwaiter().GetResult();
        WritePodmanResult(result);
      }
      finally {
        if (ownsStream)
          stream.Dispose();
      }
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPutContainerArchive")]
[OutputType(typeof(void))]
public sealed class InvokePodmanPutContainerArchiveCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  [Alias("Name", "Id")]
  public string ContainerId { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Path { get; set; } = null!;

  [Parameter]
  public string? FilePath { get; set; }

  [Parameter]
  public Stream? InputStream { get; set; }

  [Parameter]
  public SwitchParameter Pause { get; set; } = true;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var ownsStream = InputStream is null;
      var stream = OpenInputStream(FilePath, InputStream);
      try {
        var result = client.PutContainerArchiveAsync(ContainerId, stream, Path, Pause).GetAwaiter().GetResult();
        WritePodmanResult(result);
      }
      finally {
        if (ownsStream)
          stream.Dispose();
      }
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanContainerAttach")]
[OutputType(typeof(Stream), typeof(string))]
public sealed class InvokePodmanContainerAttachCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Logs { get; set; }

  [Parameter]
  public SwitchParameter Stream { get; set; } = true;

  [Parameter]
  public SwitchParameter Stdout { get; set; } = true;

  [Parameter]
  public SwitchParameter Stderr { get; set; } = true;

  [Parameter]
  public SwitchParameter Stdin { get; set; }

  [Parameter]
  public string? DetachKeys { get; set; }

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.AttachContainerAsync(
        Name,
        Logs,
        Stream,
        Stdout,
        Stderr,
        Stdin,
        DetachKeys).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanContainerSession")]
[OutputType(typeof(string), typeof(IPodmanAttachSession))]
public sealed class InvokePodmanContainerSessionCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Logs { get; set; }

  [Parameter]
  public SwitchParameter Stream { get; set; } = true;

  [Parameter]
  public SwitchParameter Stdout { get; set; } = true;

  [Parameter]
  public SwitchParameter Stderr { get; set; } = true;

  [Parameter]
  public SwitchParameter Stdin { get; set; } = true;

  [Parameter]
  public SwitchParameter Tty { get; set; }

  [Parameter]
  public string? DetachKeys { get; set; }

  [Parameter]
  public bool CollectOutput { get; set; } = true;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.AttachContainerSessionAsync(
        Name,
        Logs,
        Stream,
        Stdout,
        Stderr,
        Stdin,
        Tty,
        DetachKeys).GetAwaiter().GetResult();

      if (!result.IsSuccess) {
        WritePodmanResult(result);
        return;
      }

      if (result.Value is null)
        return;

      if (!CollectOutput) {
        WriteObject(result.Value);
        return;
      }

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

[Cmdlet(VerbsCommon.Get, "PodmanContainerChange")]
[OutputType(typeof(ContainerChangesDto))]
public sealed class GetPodmanContainerChangeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetContainerChangesAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanCommitContainer")]
[OutputType(typeof(ContainerCommitDto))]
public sealed class InvokePodmanCommitContainerCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("Name", "ContainerId", "Id")]
  public string Container { get; set; } = null!;

  [Parameter]
  public string? Repo { get; set; }

  [Parameter]
  public string? Tag { get; set; }

  [Parameter]
  public string? Comment { get; set; }

  [Parameter]
  public string? Author { get; set; }

  [Parameter]
  public SwitchParameter Pause { get; set; } = true;

  [Parameter]
  public string[]? Changes { get; set; }

  [Parameter]
  public string? Format { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.CommitContainerAsync(
        Container,
        Repo,
        Tag,
        Comment,
        Author,
        Pause,
        Changes,
        Format).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanContainerHealthCheck")]
[OutputType(typeof(ContainerHealthCheckDto))]
public sealed class InvokePodmanContainerHealthCheckCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.HealthCheckContainerAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanMountedContainer")]
[OutputType(typeof(Dictionary<string, string>))]
public sealed class GetPodmanMountedContainerCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetMountedContainersAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerProcess")]
[OutputType(typeof(ContainerTopDto))]
public sealed class GetPodmanContainerProcessCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string PsArgs { get; set; } = "-ef";

  [Parameter]
  public SwitchParameter Stream { get; set; } = true;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.TopContainerAsync(Name, PsArgs, Stream).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanContainerTop")]
[OutputType(typeof(ContainerTopDto))]
public sealed class GetPodmanContainerTopCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("ContainerId", "Id")]
  public string Name { get; set; } = null!;

  [Parameter]
  public string PsArgs { get; set; } = "-ef";

  [Parameter]
  public SwitchParameter Stream { get; set; } = true;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.TopContainerAsync(Name, PsArgs, Stream).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
