using System.Globalization;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

[PublicAPI]
public class DoubleToStringConverter : StructJsonConverter<double>
{
    protected override void WriteValue(JsonWriter writer, double value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
    }

    protected override double ParseValue(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonToken.Float => Convert.ToDouble(reader.Value!, CultureInfo.InvariantCulture),
                JsonToken.Integer => Convert.ToDouble(reader.Value!, CultureInfo.InvariantCulture),
                JsonToken.String => double.Parse((string) reader.Value!, CultureInfo.InvariantCulture),
                _ => throw JsonConverterException.Create(
                    reader,
                    "Can't convert value to double. Type of value is not string, integer, or float.",
                    typeof(DoubleToStringConverter),
                    null)
            };
        }
        catch (Exception ex) when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                "Error while converting value to double. See inner exception.",
                typeof(DoubleToStringConverter),
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
public class DoubleToStringNullableConverter : JsonConverter<double?>
{
    public override void WriteJson(JsonWriter writer, double? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString(CultureInfo.InvariantCulture));
    }

    public override double? ReadJson(JsonReader reader, Type objectType, double? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Float)
            {
                return (double) reader.Value!;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string) reader.Value!;
                return double.Parse(value, CultureInfo.InvariantCulture);
            }

            throw JsonConverterException.Create(reader,
                "Can't convert value to double. Type of value is not string", typeof(DoubleToStringNullableConverter),
                null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to double. See inner exception.",
                typeof(DoubleToStringNullableConverter), ex);
        }
    }
}