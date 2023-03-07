using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Default;

public class JsonTimeSpanTypedConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, JsonTimeSpan.FromTimeSpan(value));

    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not JsonTimeSpan value)
            throw new NullReferenceException("JsonReader value was null or not long");


        return value.ToTimeSpan();
    }
}