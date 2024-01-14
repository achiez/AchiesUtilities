using AchiesUtilities.Models;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

public class UnixTimeStampConverter : JsonConverter<UnixTimeStamp>
{
    public const UnixFormat DEFAULT_FORMAT = UnixFormat.Seconds;
    /// <summary>
    /// If <see langword="null"/> - <see cref="DEFAULT_FORMAT"/> (<see cref="UnixFormat.Seconds"/>) will be used
    /// </summary>
    public UnixFormat? Format { get; set; }

    public UnixTimeStampConverter(UnixFormat? format)
    {
        Format = format;
    }

    public UnixTimeStampConverter() { }

    public override void WriteJson(JsonWriter writer, UnixTimeStamp value, JsonSerializer serializer)
    {
        var timespanFormatted = value.ToLong(Format ?? DEFAULT_FORMAT);
        writer.WriteValue(timespanFormatted);
    }

    public override UnixTimeStamp ReadJson(JsonReader reader, Type objectType, UnixTimeStamp existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not long value)
            throw JsonConverterException.Create(reader,
                "Error while converting value to UnixTimeStamp.", typeof(NullableUnixTimeStampConverter), null);

        return new UnixTimeStamp(value, Format ?? DEFAULT_FORMAT);
    }

}