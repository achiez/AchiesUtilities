using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

public class BoolToIntConverter : JsonConverter<bool>
{
    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
    {
        var i = value ? 1 : 0;
        writer.WriteValue(i);
    }

    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not long value)
        {
            throw JsonConverterException.Create(reader,"JsonReader value was null or not long. Error converting to bool.", typeof(BoolToIntConverter), null);
        }

        return value > 0;
    }
}