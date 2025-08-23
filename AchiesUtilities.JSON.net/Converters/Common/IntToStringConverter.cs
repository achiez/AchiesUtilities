using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class IntToStringConverter : StructJsonConverter<int>
{
    protected override void WriteValue(JsonWriter writer, int value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    protected override int ParseValue(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonToken.Integer => Convert.ToInt32(reader.Value!),
                JsonToken.String => int.Parse((string) reader.Value!),
                _ => throw JsonConverterException.Create(
                    reader,
                    "Can't convert value to Int32. Value is not string or integer.",
                    typeof(IntToStringConverter),
                    null
                )
            };
        }
        catch (Exception ex) when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to Int32.",
                typeof(IntToStringConverter),
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
    false)]
public class IntToStringNullableConverter : JsonConverter<int?>
{
    public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override int? ReadJson(JsonReader reader, Type objectType, int? existingValue, bool hasExistingValue,
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
                return (int) reader.Value!;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string) reader.Value!;
                return int.Parse(value);
            }

            throw JsonConverterException.Create(reader,
                "Can't convert value to int32. Type of value is not string", typeof(IntToStringNullableConverter),
                null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to int32.",
                typeof(IntToStringNullableConverter), ex);
        }
    }
}