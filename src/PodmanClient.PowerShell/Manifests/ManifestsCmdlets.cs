using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Manifest;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsCommon.New, "PodmanManifest")]
[OutputType(typeof(ManifestCreateDto))]
public sealed class NewPodmanManifestCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Image { get; set; }

  [Parameter]
  public SwitchParameter All { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.CreateManifestAsync(Name, Image, All.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanManifest", SupportsShouldProcess = true)]
[OutputType(typeof(void))]
public sealed class RemovePodmanManifestCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Digest { get; set; }

  protected override void ProcessRecord() {
    try {
      if (!ShouldProcess(Name, "Remove Podman manifest"))
        return;

      var client = RequireClient();
      var result = client.DeleteManifestAsync(Name, Digest).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanManifest")]
[OutputType(typeof(ManifestInspectDto))]
public sealed class GetPodmanManifestCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectManifestAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Add, "PodmanManifest")]
[OutputType(typeof(void))]
public sealed class AddPodmanManifestCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter(Position = 1)]
  public string? Image { get; set; }

  [Parameter]
  public SwitchParameter All { get; set; }

  [Parameter]
  public string? Operation { get; set; }

  [Parameter]
  public ManifestAddRequestDto? Request { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var request = Request ?? new ManifestAddRequestDto {
        Images = string.IsNullOrWhiteSpace(Image) ? null : [Image],
        Image = Image,
        All = All.IsPresent,
        Operation = Operation ?? "update",
      };
      if (Request is null && (request.Images is null || request.Images.Count == 0) && string.IsNullOrWhiteSpace(request.Image))
        throw new ArgumentException("Specify -Image or -Request.");

      if (request.Images is null && !string.IsNullOrWhiteSpace(request.Image))
        request.Images = [request.Image];
      if (string.IsNullOrWhiteSpace(request.Operation))
        request.Operation = "update";

      var result = client.AddToManifestAsync(Name, request).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Publish, "PodmanManifest")]
[OutputType(typeof(void))]
public sealed class PublishPodmanManifestCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Destination { get; set; } = null!;

  [Parameter]
  public SwitchParameter All { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PushManifestAsync(Name, Destination, All.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPushManifest")]
[OutputType(typeof(void))]
public sealed class InvokePodmanPushManifestCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Destination { get; set; } = null!;

  [Parameter]
  public SwitchParameter All { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PushManifestAsync(Name, Destination, All.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}
