using System.Net;
using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AchiesUtilities.Newtonsoft.JSON.Converters.Common;

public class IPEndPointConverter : JsonConverter<IPEndPoint>
{
    public override void WriteJson(JsonWriter writer, IPEndPoint? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var ep = value;
        writer.WriteStartObject();
        writer.WritePropertyName("Address");
        serializer.Serialize(writer, ep.Address);
        writer.WritePropertyName("Port");
        writer.WriteValue(ep.Port);
        writer.WriteEndObject();
    }

    public override IPEndPoint? ReadJson(JsonReader reader, Type objectType, IPEndPoint? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonConverterException($"Unexpected token {reader.TokenType} when parsing IPEndPoint.");
        }

        try
        {
            var jo = JObject.Load(reader);
            var address = jo["Address"]!.ToObject<IPAddress>(serializer)!;
            var port = (int) jo["Port"]!;
            return new IPEndPoint(address, port);
        }
        catch (Exception ex)
        {
            throw JsonConverterException.Create(reader, "Error parsing IPEndPoint.", typeof(IPEndPointConverter), ex);
        }
    }
}