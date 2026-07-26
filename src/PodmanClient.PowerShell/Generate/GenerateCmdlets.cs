using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Generate;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsLifecycle.Invoke, "PodmanGenerateSystemd")]
[OutputType(typeof(GenerateSystemdDto))]
public sealed class InvokePodmanGenerateSystemdCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter UseName { get; set; }

  [Parameter]
  public SwitchParameter CreateNew { get; set; }

  [Parameter]
  public int? RestartSec { get; set; }

  [Parameter]
  public string? RestartPolicy { get; set; }

  [Parameter]
  public string? ContainerPrefix { get; set; }

  [Parameter]
  public string? PodPrefix { get; set; }

  [Parameter]
  public string? Separator { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GenerateSystemdAsync(
        Name,
        UseName.IsPresent,
        CreateNew.IsPresent,
        RestartSec,
        RestartPolicy,
        ContainerPrefix,
        PodPrefix,
        Separator
      ).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanGenerateKube")]
[OutputType(typeof(string))]
public sealed class InvokePodmanGenerateKubeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
  public string[] Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Service { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GenerateKubeAsync(Name, Service.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPlayKube", DefaultParameterSetName = "Path")]
[OutputType(typeof(PlayKubeReportDto))]
public sealed class InvokePodmanPlayKubeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, ParameterSetName = "Path", Position = 0)]
  public string? Path { get; set; }

  [Parameter(Mandatory = true, ParameterSetName = "InputStream")]
  public Stream? InputStream { get; set; }

  [Parameter]
  public string? Network { get; set; }

  [Parameter]
  public SwitchParameter TlsVerify { get; set; } = true;

  [Parameter]
  public SwitchParameter Start { get; set; } = true;

  [Parameter]
  public string? LogDriver { get; set; }

  protected override void ProcessRecord() {
    Stream? owned = null;
    try {
      var client = RequireClient();
      var yaml = OpenInputStream(Path, InputStream);
      if (!ReferenceEquals(yaml, InputStream))
        owned = yaml;

      var result = client.PlayKubeAsync(
        yaml,
        Network,
        TlsVerify.IsPresent,
        Start.IsPresent,
        LogDriver
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
}
