using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

public class JsonTimespanConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Ticks);
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not long value)
            throw JsonConverterException.Create(reader,
                "Error while converting value to TimeSpan. Value was null or not long.", typeof(JsonTimespanConverter),
                null);


        return new TimeSpan(value);
    }
}