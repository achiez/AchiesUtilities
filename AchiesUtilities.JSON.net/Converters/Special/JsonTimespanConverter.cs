using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

[PublicAPI]
public class JsonTimespanConverter : StructJsonConverter<TimeSpan>
{
    protected override void WriteValue(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Ticks);
    }

    protected override TimeSpan ParseValue(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.Value is not long ticks)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to TimeSpan. Value was null or not long.",
                typeof(JsonTimespanConverter),
                null
            );
        }

        return new TimeSpan(ticks);
    }
}