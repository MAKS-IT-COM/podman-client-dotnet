namespace MaksIT.PodmanClientDotNet.Streaming;

/// <summary>
/// Streaming progress events from pull/build and similar NDJSON endpoints.
/// </summary>
public interface IPodmanProgressSession<T> : IAsyncDisposable {
  IAsyncEnumerable<T> ReadProgressAsync(CancellationToken cancellationToken = default);
}
