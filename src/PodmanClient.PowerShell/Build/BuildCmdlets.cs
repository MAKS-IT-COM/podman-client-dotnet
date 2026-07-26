using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Build;
using MaksIT.PodmanClientDotNet.Streaming;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsLifecycle.Invoke, "PodmanBuildImage")]
[OutputType(typeof(BuildReportDto))]
public sealed class InvokePodmanBuildImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Dockerfile { get; set; } = null!;

  [Parameter]
  public string? ContextPath { get; set; }

  [Parameter]
  public Stream? ContextStream { get; set; }

  [Parameter]
  public SwitchParameter Pull { get; set; }

  [Parameter]
  public SwitchParameter Rm { get; set; } = true;

  [Parameter]
  public SwitchParameter ForceRm { get; set; }

  [Parameter]
  public SwitchParameter NoCache { get; set; }

  [Parameter]
  public string? Remote { get; set; }

  [Parameter]
  [Alias("t")]
  public string? Tag { get; set; }

  [Parameter]
  public string? Platform { get; set; }

  [Parameter]
  public string? BuildArgs { get; set; }

  [Parameter]
  public string? Labels { get; set; }

  protected override void ProcessRecord() {
    Stream? owned = null;
    try {
      var client = RequireClient();
      var context = ResolveContext(ref owned);
      var result = client.BuildImageAsync(
        Dockerfile,
        context,
        Pull.IsPresent,
        Rm.IsPresent,
        ForceRm.IsPresent,
        NoCache.IsPresent,
        Remote,
        Tag,
        Platform,
        BuildArgs,
        Labels
      ).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
    finally {
      owned?.Dispose();
    }
  }

  private Stream? ResolveContext(ref Stream? owned) {
    if (ContextStream is not null)
      return ContextStream;

    if (string.IsNullOrWhiteSpace(ContextPath))
      return null;

    owned = File.OpenRead(ContextPath);
    return owned;
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanBuildImageProgress")]
[OutputType(typeof(BuildProgressLineDto))]
[OutputType(typeof(IPodmanProgressSession<BuildProgressLineDto>))]
public sealed class InvokePodmanBuildImageProgressCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Dockerfile { get; set; } = null!;

  [Parameter]
  public string? ContextPath { get; set; }

  [Parameter]
  public Stream? ContextStream { get; set; }

  [Parameter]
  public SwitchParameter Pull { get; set; }

  [Parameter]
  public SwitchParameter Rm { get; set; } = true;

  [Parameter]
  public SwitchParameter ForceRm { get; set; }

  [Parameter]
  public SwitchParameter NoCache { get; set; }

  [Parameter]
  public string? Remote { get; set; }

  [Parameter]
  [Alias("t")]
  public string? Tag { get; set; }

  [Parameter]
  public string? Platform { get; set; }

  [Parameter]
  public string? BuildArgs { get; set; }

  [Parameter]
  public string? Labels { get; set; }

  [Parameter]
  public SwitchParameter Wait { get; set; } = true;

  protected override void ProcessRecord() {
    Stream? owned = null;
    try {
      var client = RequireClient();
      var context = ResolveContext(ref owned);
      var result = client.BuildImageWithProgressAsync(
        Dockerfile,
        context,
        Pull.IsPresent,
        Rm.IsPresent,
        ForceRm.IsPresent,
        NoCache.IsPresent,
        Remote,
        Tag,
        Platform,
        BuildArgs,
        Labels
      ).GetAwaiter().GetResult();

      if (!Wait.IsPresent) {
        WritePodmanResult(result);
        return;
      }

      if (!result.IsSuccess) {
        WritePodmanResult(result);
        return;
      }

      if (result.Value is null)
        return;

      var items = CollectProgress(result.Value);
      WriteObject(items, true);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
    finally {
      owned?.Dispose();
    }
  }

  private Stream? ResolveContext(ref Stream? owned) {
    if (ContextStream is not null)
      return ContextStream;

    if (string.IsNullOrWhiteSpace(ContextPath))
      return null;

    owned = File.OpenRead(ContextPath);
    return owned;
  }
}
