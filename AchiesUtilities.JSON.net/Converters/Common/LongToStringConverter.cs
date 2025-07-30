using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class LongToStringConverter : StructJsonConverter<long>
{
    protected override void WriteValue(JsonWriter writer, long value)
    {
        writer.WriteValue(value.ToString());
    }

    protected override long ParseValue(JsonReader reader)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonToken.Integer => (long)reader.Value!,
                JsonToken.String => long.Parse((string)reader.Value!),
                _ => throw JsonConverterException.Create(
                    reader,
                    "Can't convert value to Int64. Type of value is not string or integer.",
                    typeof(LongToStringConverter),
                    null
                )
            };
        }
        catch (Exception ex) when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to Int64.",
                typeof(LongToStringConverter),
                ex
            );
        }
    }
}


[PublicAPI]
[Obsolete(
    "This Nullable converter is deprecated and will be removed in future versions. " +
    "Use the corresponding non-nullable converter instead; all new converters now support both " +
    "nullable and non-nullable types automatically.",
    error: false)]
public class LongToStringNullableConverter : JsonConverter<long?>
{
    public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Integer)
            {
                return (long) reader.Value!;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string) reader.Value!;
                return long.Parse(value);
            }

            throw JsonConverterException.Create(reader,
                "Can't convert value to int64. Type of value is not string", typeof(LongToStringNullableConverter),
                null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to int64.",
                typeof(LongToStringNullableConverter), ex);
        }
    }
}