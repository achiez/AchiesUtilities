using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

public class JsonTimeSpanTypedConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, JsonTimeSpan.FromTimeSpan(value));
    }

    //FIXME: read() is not suitable. 1. It can cause deadlocks 2. JsonConverterException won't work there
    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        while (reader.TokenType != JsonToken.Integer)
        {
            reader.Read();
        }

        var ticks = (long) reader.Value!;
        reader.Read();
        return new TimeSpan(ticks);
    }
}