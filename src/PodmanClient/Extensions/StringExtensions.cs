using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaksIT.PodmanClientDotNet.Extensions;

public static partial class StringExtensions {

  
  /// <summary>
  /// Converts JSON string to object
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="s"></param>
  /// <returns></returns>
  public static T? ToObject<T>(this string s) => ToObjectCore<T>(s, null);

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="s"></param>
  /// <param name="converters"></param>
  /// <returns></returns>
  public static T? ToObject<T>(this string s, List<JsonConverter> converters) => ToObjectCore<T>(s, converters);

  private static T? ToObjectCore<T>(string s, List<JsonConverter>? converters) {
    var options = new JsonSerializerOptions {
      PropertyNameCaseInsensitive = true
    };

    converters?.ForEach(x => options.Converters.Add(x));

    return JsonSerializer.Deserialize<T>(s, options);
  }
}