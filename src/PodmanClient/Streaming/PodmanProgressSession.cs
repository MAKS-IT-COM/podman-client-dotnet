using System.Text.Json;

using MaksIT.Core.Extensions;

namespace MaksIT.PodmanClientDotNet.Streaming;

internal sealed class PodmanProgressSession<T> : IPodmanProgressSession<T> {
  private readonly Stream _stream;
  private readonly bool _ownsStream;

  internal PodmanProgressSession(Stream stream, bool ownsStream = true) {
    _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    _ownsStream = ownsStream;
  }

  public async IAsyncEnumerable<T> ReadProgressAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default) {
    using var reader = new StreamReader(_stream, leaveOpen: true);
    while (!cancellationToken.IsCancellationRequested) {
      var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
      if (line is null)
        yield break;
      if (string.IsNullOrWhiteSpace(line))
        continue;

      T? item;
      try {
        item = line.ToObject<T>();
      }
      catch (JsonException) {
        continue;
      }

      if (item is not null)
        yield return item;
    }
  }

  public async ValueTask DisposeAsync() {
    if (_ownsStream)
      await _stream.DisposeAsync().ConfigureAwait(false);
  }
}
