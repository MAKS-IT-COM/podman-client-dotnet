using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using MaksIT.PodmanClientDotNet.Internal;
using MaksIT.Results;

public partial class PodmanClient {
  private string LibpodPath(string path) {
    if (!path.StartsWith('/'))
      path = "/" + path;
    return $"/{_apiVersion}{path}";
  }

  internal static string BuildQuery(IEnumerable<(string Key, string? Value)> parameters) {
    var parts = parameters
      .Where(p => p.Value is not null)
      .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!)}")
      .ToList();
    return parts.Count == 0 ? string.Empty : "?" + string.Join("&", parts);
  }

  internal async Task<Result<T?>> SendAsync<T>(
    Func<Task<HttpResponseMessage>> send,
    string operation,
    Func<string, T?>? deserialize = null,
    CancellationToken cancellationToken = default
  ) {
    using var response = await send().ConfigureAwait(false);
    var body = response.Content is null
      ? string.Empty
      : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

    if (response.IsSuccessStatusCode) {
      if (deserialize is null || string.IsNullOrWhiteSpace(body))
        return PodmanHttpResults.Success<T>(response.StatusCode, default);

      return PodmanHttpResults.Success(response.StatusCode, deserialize(body));
    }

    var message = PodmanHttpResults.GetErrorMessage(body);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, operation, message);
    return PodmanHttpResults.Failure<T>(response.StatusCode, message);
  }

  internal async Task<Result> SendWithoutBodyAsync(
    Func<Task<HttpResponseMessage>> send,
    string operation,
    CancellationToken cancellationToken = default
  ) {
    var result = await SendAsync<object?>(send, operation, cancellationToken: cancellationToken).ConfigureAwait(false);
    return result.IsSuccess ? PodmanHttpResults.Success(result.StatusCode) : result.ToResult();
  }

  internal Task<Result<T?>> GetJsonAsync<T>(
    string libpodPath,
    string operation,
    JsonTypeInfo<T> typeInfo,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) =>
    SendAsync<T>(
      () => _httpClient.GetAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), cancellationToken),
      operation,
      body => JsonSerializer.Deserialize(body, typeInfo),
      cancellationToken
    );

  internal Task<Result> GetWithoutBodyAsync(
    string libpodPath,
    string operation,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) =>
    SendWithoutBodyAsync(
      () => _httpClient.GetAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), cancellationToken),
      operation,
      cancellationToken
    );

  internal Task<Result<TResponse?>> PostJsonAsync<TRequest, TResponse>(
    string libpodPath,
    string operation,
    TRequest? requestBody,
    JsonTypeInfo<TRequest> requestTypeInfo,
    JsonTypeInfo<TResponse> responseTypeInfo,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) {
    var content = requestBody is null
      ? null
      : new StringContent(JsonSerializer.Serialize(requestBody, requestTypeInfo), Encoding.UTF8, "application/json");

    return SendAsync<TResponse>(
      () => _httpClient.PostAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), content, cancellationToken),
      operation,
      body => string.IsNullOrWhiteSpace(body) ? default : JsonSerializer.Deserialize(body, responseTypeInfo),
      cancellationToken
    );
  }

  internal Task<Result> PostJsonWithoutBodyAsync<TRequest>(
    string libpodPath,
    string operation,
    TRequest? requestBody,
    JsonTypeInfo<TRequest> requestTypeInfo,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) {
    var content = requestBody is null
      ? null
      : new StringContent(JsonSerializer.Serialize(requestBody, requestTypeInfo), Encoding.UTF8, "application/json");

    return SendWithoutBodyAsync(
      () => _httpClient.PostAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), content, cancellationToken),
      operation,
      cancellationToken
    );
  }

  internal Task<Result> PostWithoutBodyAsync(
    string libpodPath,
    string operation,
    HttpContent? content = null,
    IEnumerable<(string Key, string? Value)>? query = null,
    string? registryAuthHeader = null,
    CancellationToken cancellationToken = default
  ) =>
    PostWithoutBodyCoreAsync(
      libpodPath,
      operation,
      content,
      query,
      registryAuthHeader,
      cancellationToken
    );

  private async Task<Result> PostWithoutBodyCoreAsync(
    string libpodPath,
    string operation,
    HttpContent? content,
    IEnumerable<(string Key, string? Value)>? query,
    string? registryAuthHeader,
    CancellationToken cancellationToken
  ) {
    using var request = CreateRequest(
      HttpMethod.Post,
      LibpodPath(libpodPath) + BuildQuery(query ?? []),
      content,
      registryAuthHeader
    );
    return await SendWithoutBodyAsync(
      () => _httpClient.SendAsync(request, cancellationToken),
      operation,
      cancellationToken
    ).ConfigureAwait(false);
  }

  internal Task<Result<TResponse?>> PostLibpodAsync<TResponse>(
    string libpodPath,
    string operation,
    JsonTypeInfo<TResponse> responseTypeInfo,
    HttpContent? content = null,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) =>
    SendAsync<TResponse>(
      () => _httpClient.PostAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), content, cancellationToken),
      operation,
      body => string.IsNullOrWhiteSpace(body) ? default : JsonSerializer.Deserialize(body, responseTypeInfo),
      cancellationToken
    );

  internal Task<Result<T?>> DeleteJsonAsync<T>(
    string libpodPath,
    string operation,
    JsonTypeInfo<T> typeInfo,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) =>
    SendAsync<T>(
      () => _httpClient.DeleteAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), cancellationToken),
      operation,
      body => string.IsNullOrWhiteSpace(body) ? default : JsonSerializer.Deserialize(body, typeInfo),
      cancellationToken
    );

  internal Task<Result> DeleteWithoutBodyAsync(
    string libpodPath,
    string operation,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) =>
    SendWithoutBodyAsync(
      () => _httpClient.DeleteAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), cancellationToken),
      operation,
      cancellationToken
    );

  internal Task<Result> PutWithoutBodyAsync(
    string libpodPath,
    string operation,
    HttpContent? content = null,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) =>
    SendWithoutBodyAsync(
      () => _httpClient.PutAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), content, cancellationToken),
      operation,
      cancellationToken
    );

  private static HttpRequestMessage CreateRequest(
    HttpMethod method,
    string requestUri,
    HttpContent? content = null,
    string? registryAuthHeader = null
  ) {
    var request = new HttpRequestMessage(method, requestUri) { Content = content };
    if (!string.IsNullOrEmpty(registryAuthHeader))
      request.Headers.TryAddWithoutValidation("X-Registry-Auth", registryAuthHeader);
    return request;
  }

  internal async Task<Result<Stream?>> PostStreamAsync(
    string libpodPath,
    string operation,
    HttpContent? content = null,
    IEnumerable<(string Key, string? Value)>? query = null,
    string? registryAuthHeader = null,
    CancellationToken cancellationToken = default
  ) {
    using var request = CreateRequest(
      HttpMethod.Post,
      LibpodPath(libpodPath) + BuildQuery(query ?? []),
      content,
      registryAuthHeader
    );
    var response = await _httpClient
      .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
      .ConfigureAwait(false);

    if (response.IsSuccessStatusCode) {
      var inner = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
      return PodmanHttpResults.Success<Stream?>(
        response.StatusCode,
        new PodmanOwnedResponseStream(inner, response)
      );
    }

    var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    var message = PodmanHttpResults.GetErrorMessage(body);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, operation, message);
    response.Dispose();
    return PodmanHttpResults.Failure<Stream?>(response.StatusCode, message);
  }

  internal async Task<Result<Stream?>> GetStreamAsync(
    string libpodPath,
    string operation,
    IEnumerable<(string Key, string? Value)>? query = null,
    CancellationToken cancellationToken = default
  ) {
    var response = await _httpClient
      .GetAsync(LibpodPath(libpodPath) + BuildQuery(query ?? []), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
      .ConfigureAwait(false);

    if (response.IsSuccessStatusCode) {
      var inner = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
      return PodmanHttpResults.Success<Stream?>(
        response.StatusCode,
        new PodmanOwnedResponseStream(inner, response)
      );
    }

    var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    var message = PodmanHttpResults.GetErrorMessage(body);
    PodmanHttpResults.LogFailure(_logger, response.StatusCode, operation, message);
    response.Dispose();
    return PodmanHttpResults.Failure<Stream?>(response.StatusCode, message);
  }
}
