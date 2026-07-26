using System.Management.Automation;

using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.PodmanClientDotNet.Dtos.Image;
using MaksIT.PodmanClientDotNet.Streaming;


namespace MaksIT.PodmanClientDotNet.PowerShell;

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPullImage")]
public sealed class InvokePodmanPullImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Reference { get; set; } = null!;

  [Parameter]
  public bool TlsVerify { get; set; } = true;

  [Parameter]
  public SwitchParameter Quiet { get; set; }

  [Parameter]
  public string Policy { get; set; } = "always";

  [Parameter]
  public string? Arch { get; set; }

  [Parameter]
  public string? Os { get; set; }

  [Parameter]
  public string? Variant { get; set; }

  [Parameter]
  public SwitchParameter AllTags { get; set; }

  [Parameter]
  public string? AuthHeader { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PullImageAsync(
        Reference,
        TlsVerify,
        Quiet.IsPresent,
        Policy,
        Arch,
        Os,
        Variant,
        AllTags.IsPresent,
        AuthHeader).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanTagImage")]
public sealed class InvokePodmanTagImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Image { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 1)]
  public string Repo { get; set; } = null!;

  [Parameter(Mandatory = true, Position = 2)]
  public string Tag { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.TagImageAsync(Image, Repo, Tag).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanImageList")]
[OutputType(typeof(ImageListEntryDto))]
public sealed class GetPodmanImageListCmdlet : PodmanCmdletBase {
  [Parameter]
  public SwitchParameter All { get; set; }

  [Parameter]
  public string? Filters { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ListImagesAsync(All.IsPresent, Filters).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanImage")]
[OutputType(typeof(ImageInspectDto))]
public sealed class GetPodmanImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.InspectImageAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsDiagnostic.Test, "PodmanImage")]
[OutputType(typeof(bool))]
public sealed class TestPodmanImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ImageExistsAsync(Name).GetAwaiter().GetResult();
      WriteObject(result.IsSuccess);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanImage")]
[OutputType(typeof(ImageDeleteDto))]
public sealed class RemovePodmanImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter]
  public SwitchParameter Force { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.DeleteImageAsync(Name, Force.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Remove, "PodmanImageBatch")]
[OutputType(typeof(ImageDeleteDto))]
public sealed class RemovePodmanImageBatchCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true)]
  public string[] Image { get; set; } = null!;

  [Parameter]
  public SwitchParameter All { get; set; }

  [Parameter]
  public SwitchParameter Force { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.RemoveImagesAsync(Image, All.IsPresent, Force.IsPresent).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPruneImage")]
[OutputType(typeof(PruneReportEntryDto))]
public sealed class InvokePodmanPruneImageCmdlet : PodmanCmdletBase {
  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PruneImagesAsync().GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Search, "PodmanImage")]
[OutputType(typeof(ImageSearchResultDto))]
public sealed class SearchPodmanImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Term { get; set; } = null!;

  [Parameter]
  public int? Limit { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.SearchImagesAsync(Term, Limit).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPushImage")]
public sealed class InvokePodmanPushImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Destination { get; set; }

  [Parameter]
  public bool TlsVerify { get; set; } = true;

  [Parameter]
  public SwitchParameter Compress { get; set; }

  [Parameter]
  public string? AuthHeader { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PushImageAsync(Name, Destination, TlsVerify, Compress.IsPresent, AuthHeader)
        .GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanUntagImage")]
public sealed class InvokePodmanUntagImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Repo { get; set; }

  [Parameter]
  public string? Tag { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.UntagImageAsync(Name, Repo, Tag).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanImageHistory")]
[OutputType(typeof(ImageHistoryEntryDto))]
public sealed class GetPodmanImageHistoryCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetImageHistoryAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanImageTree")]
[OutputType(typeof(ImageTreeDto))]
public sealed class GetPodmanImageTreeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetImageTreeAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsCommon.Get, "PodmanImageChange")]
[OutputType(typeof(ImageChangesDto))]
public sealed class GetPodmanImageChangeCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetImageChangesAsync(Name).GetAwaiter().GetResult();
      WritePodmanResult(result);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Import, "PodmanImage")]
[OutputType(typeof(ImageImportDto))]
public sealed class ImportPodmanImageCmdlet : PodmanCmdletBase {
  [Parameter]
  public string? Path { get; set; }

  [Parameter]
  public Stream? InputStream { get; set; }

  [Parameter]
  public string? Changes { get; set; }

  [Parameter]
  public string? Message { get; set; }

  [Parameter]
  public string? Reference { get; set; }

  [Parameter]
  public string? Url { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      Stream? stream = null;
      var ownsStream = false;
      if (InputStream is not null || !string.IsNullOrWhiteSpace(Path)) {
        stream = OpenInputStream(Path, InputStream);
        ownsStream = InputStream is null;
      }

      try {
        var result = client.ImportImageAsync(stream, Changes, Message, Reference, Url)
          .GetAwaiter().GetResult();
        WritePodmanResult(result);
      }
      finally {
        if (ownsStream)
          stream?.Dispose();
      }
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Import, "PodmanImageArchive")]
[OutputType(typeof(ImageLoadDto))]
public sealed class ImportPodmanImageArchiveCmdlet : PodmanCmdletBase {
  [Parameter]
  public string? Path { get; set; }

  [Parameter]
  public Stream? InputStream { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var stream = OpenInputStream(Path, InputStream);
      var ownsStream = InputStream is null;
      try {
        var result = client.LoadImageAsync(stream).GetAwaiter().GetResult();
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

[Cmdlet(VerbsData.Export, "PodmanImage")]
[OutputType(typeof(Stream))]
[OutputType(typeof(string))]
public sealed class ExportPodmanImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true)]
  public string[] Reference { get; set; } = null!;

  [Parameter]
  public string? Format { get; set; }

  [Parameter]
  public SwitchParameter Compress { get; set; }

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.ExportImagesAsync(Reference, Format, Compress.IsPresent).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsData.Save, "PodmanImage")]
[OutputType(typeof(Stream))]
[OutputType(typeof(string))]
public sealed class SavePodmanImageCmdlet : PodmanCmdletBase {
  [Parameter(Mandatory = true, Position = 0)]
  public string Name { get; set; } = null!;

  [Parameter]
  public string? Format { get; set; }

  [Parameter]
  public SwitchParameter Compress { get; set; }

  [Parameter]
  public string? OutFile { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.GetImageAsync(Name, Format, Compress.IsPresent).GetAwaiter().GetResult();
      WritePodmanStream(result, OutFile);
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "PodmanApiError", ErrorCategory.NotSpecified, null));
    }
  }
}

[Cmdlet(VerbsLifecycle.Invoke, "PodmanPullImageProgress")]
[OutputType(typeof(PullImageResponseDto))]
[OutputType(typeof(IPodmanProgressSession<PullImageResponseDto>))]
public sealed class InvokePodmanPullImageProgressCmdlet : PodmanCmdletBase {
  public InvokePodmanPullImageProgressCmdlet() {
    Wait = true;
  }

  [Parameter(Mandatory = true, Position = 0)]
  public string Reference { get; set; } = null!;

  [Parameter]
  public bool TlsVerify { get; set; } = true;

  [Parameter]
  public SwitchParameter Quiet { get; set; }

  [Parameter]
  public string Policy { get; set; } = "always";

  [Parameter]
  public string? Arch { get; set; }

  [Parameter]
  public string? Os { get; set; }

  [Parameter]
  public string? Variant { get; set; }

  [Parameter]
  public SwitchParameter AllTags { get; set; }

  [Parameter]
  public string? AuthHeader { get; set; }

  [Parameter]
  public SwitchParameter Wait { get; set; }

  protected override void ProcessRecord() {
    try {
      var client = RequireClient();
      var result = client.PullImageWithProgressAsync(
        Reference,
        TlsVerify,
        Quiet.IsPresent,
        Policy,
        Arch,
        Os,
        Variant,
        AllTags.IsPresent,
        AuthHeader).GetAwaiter().GetResult();

      if (!Wait) {
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
        foreach (var item in CollectProgress(result.Value))
          WriteObject(item);
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
