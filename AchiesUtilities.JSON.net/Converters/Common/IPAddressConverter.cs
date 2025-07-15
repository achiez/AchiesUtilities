using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

public class IPAddressConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IPAddress));
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        IPAddress ip = (IPAddress)value;
        writer.WriteValue(ip.ToString());
    }

    /// <inheritdoc/>
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null!;
        }
        JToken token = JToken.Load(reader);
        return IPAddress.Parse(token.Value<string>());
    }
}