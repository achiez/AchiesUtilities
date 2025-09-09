using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class UlongToStringConverter : StructJsonConverter<ulong>
{
    protected override void WriteValue(JsonWriter writer, ulong value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    protected override ulong ParseValue(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonToken.Integer => (ulong) reader.Value!,
                JsonToken.String => ulong.Parse((string) reader.Value!),
                _ => throw JsonConverterException.Create(
                    reader,
                    "Can't convert value to Int64. Type of value is not string or integer.",
                    typeof(UlongToStringConverter),
                    null
                )
            };
        }
        catch (Exception ex) when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to Int64.",
                typeof(UlongToStringConverter),
                ex
            );
        }
    }
}