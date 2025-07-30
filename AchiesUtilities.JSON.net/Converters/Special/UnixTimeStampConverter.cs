using AchiesUtilities.Models;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

[PublicAPI]
public class UnixTimeStampConverter : StructJsonConverter<UnixTimeStamp>
{
    public const UnixFormat DEFAULT_FORMAT = UnixFormat.Seconds;

    /// <summary>
    ///     If <see langword="null" /> - <see cref="DEFAULT_FORMAT" /> (<see cref="UnixFormat.Seconds" />) will be used
    /// </summary>
    public UnixFormat? Format { get; set; }

    public UnixTimeStampConverter(UnixFormat? format = null)
    {
        Format = format;
    }

    protected override void WriteValue(JsonWriter writer, UnixTimeStamp value)
    {
        var timespanFormatted = value.ToLong(Format ?? DEFAULT_FORMAT);
        writer.WriteValue(timespanFormatted);
    }

    protected override UnixTimeStamp ParseValue(JsonReader reader)
    {
        if (reader.Value is not long value)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to UnixTimeStamp. Value was null or not long",
                typeof(UnixTimeStampConverter),
                null
            );
        }

        return new UnixTimeStamp(value, Format ?? DEFAULT_FORMAT);
    }
}