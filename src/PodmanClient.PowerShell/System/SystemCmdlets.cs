using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.System;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsDiagnostic.Test, "PodmanConnection")]
[OutputType(typeof(LibpodPingDto))]
public sealed class TestPodmanConnectionCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PingAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanVersion")]
[OutputType(typeof(LibpodVersionDto))]
public sealed class GetPodmanVersionCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetVersionAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanInfo")]
[OutputType(typeof(InfoDto))]
public sealed class GetPodmanInfoCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetInfoAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanSystemDiskUsage")]
[OutputType(typeof(SystemDfDto))]
public sealed class GetPodmanSystemDiskUsageCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetSystemDiskUsageAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPruneSystem")]
[OutputType(typeof(SystemPruneReportDto))]
public sealed class InvokePodmanPruneSystemCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PruneSystemAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanEvent")]
[OutputType(typeof(Stream))]
[OutputType(typeof(string))]
public sealed class GetPodmanEventCmdlet : PodmanCmdletBase {
  [Parameter]
  public string? OutFile { get; set; }

  /// <summary>Max time to read from the events stream when <see cref="OutFile"/> is set (stream is otherwise open-ended).</summary>
  [Parameter]
  public int ReadTimeoutSeconds { get; set; } = 2;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetEventsAsync().GetAwaiter().GetResult();
      if (!result.IsSuccess) {
        WritePodmanResult(result);
        return;
      }

      if (result.Value is null)
        return;

      if (string.IsNullOrWhiteSpace(OutFile)) {
        WriteObject(result.Value);
        return;
      }

      using (result.Value)
      using (var fs = File.Create(OutFile)) {
        var buffer = new byte[8192];
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(Math.Max(1, ReadTimeoutSeconds)));
        try {
          while (true) {
            var read = result.Value.ReadAsync(buffer.AsMemory(0, buffer.Length), cts.Token).AsTask().GetAwaiter().GetResult();
            if (read <= 0)
              break;
            fs.Write(buffer, 0, read);
          }
        }
        catch (OperationCanceledException) {
          // Timed sample of the open-ended events stream.
        }
      }

      WriteObject(OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
