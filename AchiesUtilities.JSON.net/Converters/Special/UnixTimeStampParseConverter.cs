using AchiesUtilities.Models;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

[PublicAPI]
public class UnixTimeStampParseConverter : StructJsonConverter<UnixTimeStamp>
{
    protected override void WriteValue(JsonWriter writer, UnixTimeStamp value)
    {
        writer.WriteValue(value.Seconds);
    }

    protected override UnixTimeStamp ParseValue(JsonReader reader)
    {
        try
        {
            if (reader.Value is null)
            {
                throw JsonConverterException.Create(
                    reader,
                    $"Error while parsing {nameof(UnixTimeStamp)}. Value is null.",
                    typeof(UnixTimeStampParseConverter),
                    null
                );
            }

            return UnixTimeStamp.Parse(reader.Value);
        }
        catch (Exception e) when (e is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                $"Error while parsing {nameof(UnixTimeStamp)}.",
                typeof(UnixTimeStampParseConverter),
                e
            );
        }
    }
}
