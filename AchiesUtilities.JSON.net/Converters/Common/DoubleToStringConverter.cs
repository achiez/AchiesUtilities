    using System.Globalization;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

public class DoubleToStringConverter : JsonConverter<double>
{
    public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
    }

    public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.Float)
            {
                return (double)reader.Value!;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value!;
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
            throw JsonConverterException.Create(reader,
                "Can't convert value to double. Type of value is not string", typeof(DoubleToStringConverter), null);
        }
        catch (Exception ex)
            when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(reader, "Error while converting value to double. See inner exception.",
                typeof(DoubleToStringConverter), ex);
        }
    }
}