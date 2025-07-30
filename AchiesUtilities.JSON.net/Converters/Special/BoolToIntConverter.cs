using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Special;

[PublicAPI]
public class BoolToIntConverter : StructJsonConverter<bool>
{
    protected override void WriteValue(JsonWriter writer, bool value)
    {
        writer.WriteValue(value ? 1 : 0);
    }

    protected override bool ParseValue(JsonReader reader)
    {
        if (reader.Value is not long value)
        {
            throw JsonConverterException.Create(
                reader,
                "JsonReader value was null or not long. Error converting to bool.",
                typeof(BoolToIntConverter),
                null
            );
        }

        return value > 0;
    }
}