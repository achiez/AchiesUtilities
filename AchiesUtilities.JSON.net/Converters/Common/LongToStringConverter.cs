using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class LongToStringConverter : JsonConverter<long>
{

    public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
    {

        try
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                return (long)reader.Value!;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return long.Parse(value);
            }
            throw JsonConverterException.Create(reader,
                "Can't convert value to int64. Type of value is not string", typeof(LongToStringConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to int64.",
                typeof(LongToStringConverter), ex);
        }
    }
}


[PublicAPI]
public class LongToStringNullableConverter : JsonConverter<long?>
{

    public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {

        try
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonToken.Integer)
            {
                return (long)reader.Value!;
            }
            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return long.Parse(value);
            }
            throw JsonConverterException.Create(reader,
                "Can't convert value to int64. Type of value is not string", typeof(LongToStringNullableConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to int64.",
                typeof(LongToStringNullableConverter), ex);
        }
    }
}
