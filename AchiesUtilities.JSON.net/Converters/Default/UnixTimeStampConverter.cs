using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Default;

public class UnixTimeStampConverter : JsonConverter<UnixTimeStamp>
{
    public const UnixFormat DefaultFormat = UnixFormat.Seconds;
    /// <summary>
    /// If <see langword="null"/> - <see cref="DefaultFormat"/> (<see cref="UnixFormat.Seconds"/>) will be used
    /// </summary>
    public UnixFormat? Format { get; set; }

    public override void WriteJson(JsonWriter writer, UnixTimeStamp value, JsonSerializer serializer)
    {
        var timespanFormatted = value.ToLong(Format ?? DefaultFormat);
        writer.WriteValue(timespanFormatted);
    }

    public override UnixTimeStamp ReadJson(JsonReader reader, Type objectType, UnixTimeStamp existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not long value)
            throw new NullReferenceException("JsonReader value was null");

        return new UnixTimeStamp(value, Format ?? DefaultFormat);
    }
}