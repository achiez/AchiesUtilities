using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters;

public class TimespanConverter : JsonConverter<TimeSpan>
{
    /// <summary>
    /// Format: Days.Hours:Minutes:Seconds:Milliseconds
    /// </summary>
    public const string TimeSpanFormatString = @"d\.hh\:mm\:ss\:FFF";

    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        var timespanFormatted = $"{value.ToString(TimeSpanFormatString)}";
        writer.WriteValue(timespanFormatted);
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not string value)
            throw new NullReferenceException("JsonReader value was null");

        if(TimeSpan.TryParseExact(value, TimeSpanFormatString, null, out var parsedTimeSpan) == false)
            throw new FormatException("Can't parse TimeSpan from json format");

        return parsedTimeSpan;
    }
}