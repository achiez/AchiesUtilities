using AchiesUtilities.Newtonsoft.JSON.Converters;
using System.ComponentModel;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON;

public static class StaticConverters
{
    public static JsonTimespanConverter TimeSpanConverter { get; } = new();
    public static UnixTimeStampConverter UnixTimeStampConverterSeconds { get; } = new();
    public static IList<JsonConverter> AllConverters { get; } = new JsonConverter[]
    {
        TimeSpanConverter,
        UnixTimeStampConverterSeconds,
    };
  
}