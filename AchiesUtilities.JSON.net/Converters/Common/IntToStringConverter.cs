using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class IntToStringConverter : JsonConverter<int>
{

    public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                return (int)reader.Value!;
            }
            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return int.Parse(value);
            }
            throw JsonConverterException.Create(reader,
                "Can't convert value to int32. Type of value is not string", typeof(IntToStringConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to int32.",
                typeof(IntToStringConverter), ex);
        }
    }
}

[PublicAPI]
public class IntToStringNullableConverter : JsonConverter<int?>
{

    public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override int? ReadJson(JsonReader reader, Type objectType, int? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonToken.Integer)
            {
                return (int)reader.Value!;
            }
            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return int.Parse(value);
            }
            throw JsonConverterException.Create(reader,
                "Can't convert value to int32. Type of value is not string", typeof(IntToStringNullableConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to int32.",
                typeof(IntToStringNullableConverter), ex);
        }
    }
}