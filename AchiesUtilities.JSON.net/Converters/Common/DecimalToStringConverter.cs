using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Globalization;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class DecimalToStringConverter : JsonConverter<decimal>
{
    public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
    }

    public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Float)
            {
                var d = (double)reader.Value!;
                return Convert.ToDecimal(d);
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            throw JsonConverterException.Create(reader,
                "Can't convert value to decimal. Type of value is not string", typeof(DecimalToStringConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to decimal. See inner exception.",
                typeof(DecimalToStringConverter), ex);
        }
    }
}

[PublicAPI]
public class DecimalToStringNullableConverter : JsonConverter<decimal?>
{
    public override void WriteJson(JsonWriter writer, decimal? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString(CultureInfo.InvariantCulture));
    }

    public override decimal? ReadJson(JsonReader reader, Type objectType, decimal? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Float)
            {
                var d = (double)reader.Value!;
                return Convert.ToDecimal(d);
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            throw JsonConverterException.Create(reader,
                "Can't convert value to decimal. Type of value is not string, null or float", typeof(DecimalToStringNullableConverter), null);

        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to decimal. See inner exception.",
                typeof(DecimalToStringNullableConverter), ex);
        }
    }
}