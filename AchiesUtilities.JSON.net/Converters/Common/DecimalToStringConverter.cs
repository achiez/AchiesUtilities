using System.Globalization;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class DecimalToStringConverter : StructJsonConverter<decimal>
{
    protected override void WriteValue(JsonWriter writer, decimal value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
    }

    protected override decimal ParseValue(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonToken.Float => Convert.ToDecimal((double) reader.Value!, CultureInfo.InvariantCulture),
                JsonToken.Integer => Convert.ToDecimal(reader.Value!, CultureInfo.InvariantCulture),
                JsonToken.String => decimal.Parse((string) reader.Value!, CultureInfo.InvariantCulture),
                _ => throw JsonConverterException.Create(
                    reader,
                    "Can't convert value to decimal. Type of value is not string, integer, or float.",
                    typeof(DecimalToStringConverter),
                    null)
            };
        }
        catch (Exception ex) when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to decimal. See inner exception.",
                typeof(DecimalToStringConverter),
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
                var d = (double) reader.Value!;
                return Convert.ToDecimal(d);
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string) reader.Value!;
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            throw JsonConverterException.Create(reader,
                "Can't convert value to decimal. Type of value is not string, null or float",
                typeof(DecimalToStringNullableConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to decimal. See inner exception.",
                typeof(DecimalToStringNullableConverter), ex);
        }
    }
}