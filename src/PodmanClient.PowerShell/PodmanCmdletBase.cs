using System.Management.Automation;
using System.Text;

using MaksIT.PodmanClientDotNet.Streaming;
using MaksIT.Results;

namespace MaksIT.PodmanClientDotNet.PowerShell;

public abstract class PodmanCmdletBase : PSCmdlet {
  protected IPodmanClient RequireClient() {
    var client = PodmanConnectionState.Client;
    if (client is null) {
      ThrowTerminatingError(new ErrorRecord(
        new InvalidOperationException("Not connected. Run Connect-Podman -BaseAddress <url> first."),
        "NotConnected",
        ErrorCategory.InvalidOperation,
        null));
    }

    return client;
  }

  protected void WritePodmanResult(Result result) {
    if (result.IsSuccess)
      return;

    WriteError(new ErrorRecord(
      new InvalidOperationException(string.Join("; ", result.Messages)),
      "PodmanApiError",
      ErrorCategory.InvalidOperation,
      null));
  }

  protected void WritePodmanResult<T>(Result<T?> result) {
    if (!result.IsSuccess) {
      WriteError(new ErrorRecord(
        new InvalidOperationException(string.Join("; ", result.Messages)),
        "PodmanApiError",
        ErrorCategory.InvalidOperation,
        null));
      return;
    }

    if (result.Value is null)
      return;

    // Avoid PowerShell enumerating list results onto the host unexpectedly.
    if (result.Value is System.Collections.IEnumerable and not string and not System.Collections.IDictionary)
      WriteObject(result.Value, enumerateCollection: false);
    else
      WriteObject(result.Value);
  }

  protected void WritePodmanStream(Result<Stream?> result, string? outFile) {
    if (!result.IsSuccess) {
      WriteError(new ErrorRecord(
        new InvalidOperationException(string.Join("; ", result.Messages)),
        "PodmanApiError",
        ErrorCategory.InvalidOperation,
        null));
      return;
    }

    if (result.Value is null)
      return;

    if (!string.IsNullOrWhiteSpace(outFile)) {
      using (result.Value)
      using (var fs = File.Create(outFile))
        result.Value.CopyTo(fs);
      WriteObject(outFile);
      return;
    }

    WriteObject(result.Value);
  }

  protected static Stream OpenInputStream(string? path, Stream? inputStream) {
    if (inputStream is not null)
      return inputStream;
    if (string.IsNullOrWhiteSpace(path))
      throw new ArgumentException("Specify -Path or -InputStream.");
    return File.OpenRead(path);
  }

  protected static string CollectAttachOutput(IPodmanAttachSession session) {
    var output = new StringBuilder();
    while (true) {
      var frame = session.ReadFrameAsync().GetAwaiter().GetResult();
      if (frame is null)
        break;
      output.Append(Encoding.UTF8.GetString(frame.Data));
    }

    return output.ToString();
  }

  protected static List<T> CollectProgress<T>(IPodmanProgressSession<T> session) {
    var items = new List<T>();
    var enumerator = session.ReadProgressAsync().GetAsyncEnumerator();
    try {
      while (enumerator.MoveNextAsync().AsTask().GetAwaiter().GetResult())
        items.Add(enumerator.Current);
    }
    finally {
      enumerator.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    return items;
  }
}
