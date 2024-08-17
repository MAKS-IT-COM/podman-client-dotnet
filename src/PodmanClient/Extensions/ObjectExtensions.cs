using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaksIT.PodmanClientDotNet.Extensions;

public static class ObjectExtensions {

  /// <summary>
  /// Converts object to json string
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static string ToJson<T>(this T? obj) => ToJson(obj, null);

  /// <summary>
  /// Converts object to json string
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj"></param>
  /// <param name="converters"></param>
  /// <returns></returns>
  public static string ToJson<T>(T? obj, List<JsonConverter>? converters) {
    if (obj == null)
      return "{}";

    var options = new JsonSerializerOptions {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    converters?.ForEach(x => options.Converters.Add(x));

    return JsonSerializer.Serialize(obj, options);
  }
}