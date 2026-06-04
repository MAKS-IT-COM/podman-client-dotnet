using System.Net;

using Microsoft.Extensions.Logging;

using MaksIT.Core.Extensions;
using MaksIT.PodmanClientDotNet.Dtos.Common;
using MaksIT.Results;

namespace MaksIT.PodmanClientDotNet.Internal;

internal static class PodmanHttpResults {
  public static string GetErrorMessage(string content) {
    if (string.IsNullOrWhiteSpace(content))
      return "Podman API request failed.";

    var error = content.ToObject<ErrorResponseDto>();
    return string.IsNullOrWhiteSpace(error?.Message) ? content : error.Message;
  }

  public static Result<T?> Success<T>(HttpStatusCode statusCode, T? value, params string[] messages) =>
    statusCode switch {
      HttpStatusCode.Created => Result<T?>.Created(value, messages),
      HttpStatusCode.NoContent => Result<T?>.NoContent(value, messages),
      HttpStatusCode.Accepted => Result<T?>.Accepted(value, messages),
      _ => Result<T?>.Ok(value, messages),
    };

  public static Result Success(HttpStatusCode statusCode, params string[] messages) =>
    statusCode switch {
      HttpStatusCode.Created => Result.Created(messages),
      HttpStatusCode.NoContent => Result.NoContent(messages),
      HttpStatusCode.Accepted => Result.Accepted(messages),
      _ => Result.Ok(messages),
    };

  public static Result<T?> Failure<T>(HttpStatusCode statusCode, string message) =>
    statusCode switch {
      HttpStatusCode.BadRequest => Result<T?>.BadRequest(default, message),
      HttpStatusCode.Unauthorized => Result<T?>.Unauthorized(default, message),
      HttpStatusCode.Forbidden => Result<T?>.Forbidden(default, message),
      HttpStatusCode.NotFound => Result<T?>.NotFound(default, message),
      HttpStatusCode.Conflict => Result<T?>.Conflict(default, message),
      HttpStatusCode.InternalServerError => Result<T?>.InternalServerError(default, message),
      HttpStatusCode.ServiceUnavailable => Result<T?>.ServiceUnavailable(default, message),
      HttpStatusCode.GatewayTimeout => Result<T?>.GatewayTimeout(default, message),
      _ when (int)statusCode >= 500 => Result<T?>.InternalServerError(default, message),
      _ => Result<T?>.BadRequest(default, message),
    };

  public static Result Failure(HttpStatusCode statusCode, string message) =>
    statusCode switch {
      HttpStatusCode.BadRequest => Result.BadRequest(message),
      HttpStatusCode.Unauthorized => Result.Unauthorized(message),
      HttpStatusCode.Forbidden => Result.Forbidden(message),
      HttpStatusCode.NotFound => Result.NotFound(message),
      HttpStatusCode.Conflict => Result.Conflict(message),
      HttpStatusCode.InternalServerError => Result.InternalServerError(message),
      HttpStatusCode.ServiceUnavailable => Result.ServiceUnavailable(message),
      HttpStatusCode.GatewayTimeout => Result.GatewayTimeout(message),
      _ when (int)statusCode >= 500 => Result.InternalServerError(message),
      _ => Result.BadRequest(message),
    };

  public static void LogFailure(ILogger logger, HttpStatusCode statusCode, string operation, string message) {
    switch (statusCode) {
      case HttpStatusCode.NotFound:
        logger.LogWarning("{Operation} failed: {Message}", operation, message);
        break;
      case HttpStatusCode.Conflict:
      case HttpStatusCode.NotModified:
        logger.LogWarning("{Operation}: {Message}", operation, message);
        break;
      default:
        if ((int)statusCode >= 500)
          logger.LogError("{Operation} failed: {Message}", operation, message);
        else
          logger.LogError("{Operation} failed ({StatusCode}): {Message}", operation, (int)statusCode, message);
        break;
    }
  }
}
