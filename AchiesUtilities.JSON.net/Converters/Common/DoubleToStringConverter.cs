using System.Globalization;
using Newtonsoft.Json;

namespace SteamLib.Web.Converters;

public class StringToDoubleConverter : JsonConverter<double>
{
    public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
       var value = (string)reader.Value!;
       return double.Parse(value, CultureInfo.InvariantCulture);
    }
}