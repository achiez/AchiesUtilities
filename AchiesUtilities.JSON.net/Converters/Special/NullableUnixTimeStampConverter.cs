using AchiesUtilities.Models;
using AchiesUtilities.Newtonsoft.JSON.Converters.Common;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

public class NullableUnixTimeStampConverter : JsonConverter<UnixTimeStamp?>
{
    public const UnixFormat DEFAULT_FORMAT = UnixFormat.Seconds;

    /// <summary>
    ///     If <see langword="null" /> - <see cref="DEFAULT_FORMAT" /> (<see cref="UnixFormat.Seconds" />) will be used
    /// </summary>
    public UnixFormat? Format { get; set; }

    public NullableUnixTimeStampConverter(UnixFormat? format)
    {
        Format = format;
    }

    public NullableUnixTimeStampConverter()
    {
    }

    public override void WriteJson(JsonWriter writer, UnixTimeStamp? value, JsonSerializer serializer)
    {
        var timespanFormatted = value?.ToLong(Format ?? DEFAULT_FORMAT);
        writer.WriteValue(timespanFormatted);
    }

    public override UnixTimeStamp? ReadJson(JsonReader reader, Type objectType, UnixTimeStamp? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        try
        {
            if (reader.Value == null)
                return null;

            if (reader.TokenType != JsonToken.Integer)
            {
                throw JsonConverterException.Create(reader,
                    $"Error while converting value to nullable timestamp. JsonReader value has type {reader.TokenType} which is not supported.",
                    typeof(NullableUnixTimeStampConverter), null);
            }

            var value = (long) reader.Value;

            return new UnixTimeStamp(value, Format ?? DEFAULT_FORMAT);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader,
                "Error while converting value to nullable timestamp. See inner exception.",
                typeof(DoubleToStringConverter), ex);
        }
    }
}