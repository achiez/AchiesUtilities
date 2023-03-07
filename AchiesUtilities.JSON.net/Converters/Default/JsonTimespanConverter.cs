using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Default;

public class JsonTimespanConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Ticks);
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not long value)
            throw new NullReferenceException("JsonReader value was null or not long");


        return new TimeSpan(value);
    }
}