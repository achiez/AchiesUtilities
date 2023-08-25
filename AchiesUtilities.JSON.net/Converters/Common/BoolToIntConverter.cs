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
        if (reader.Value is not int value)
        {
            throw new NullReferenceException("JsonReader value was null or not int");
        }

        return value > 0;
    }
}