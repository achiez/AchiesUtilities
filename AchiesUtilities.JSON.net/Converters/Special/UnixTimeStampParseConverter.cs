using AchiesUtilities.Models;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

public class UnixTimeStampParseConverter : JsonConverter<UnixTimeStamp>
{
    public override void WriteJson(JsonWriter writer, UnixTimeStamp value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Seconds);
    }

    public override UnixTimeStamp ReadJson(JsonReader reader, Type objectType, UnixTimeStamp existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            return UnixTimeStamp.Parse(reader.Value!);
        }
        catch (Exception e)
        {
            throw JsonConverterException.Create(reader, $"Error while parsing {nameof(UnixTimeStamp)}.",
                typeof(UnixTimeStampParseConverter), e);
        }
    }
}