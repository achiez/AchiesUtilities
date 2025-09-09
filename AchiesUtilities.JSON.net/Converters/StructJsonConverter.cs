using AchiesUtilities.Newtonsoft.JSON.Exceptions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Converters;

[PublicAPI]
public abstract class StructJsonConverter<T> : JsonConverter where T : struct
{
    public override bool CanConvert(Type objectType)
    {
        var t = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return t == typeof(T);
    }

    public sealed override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        WriteValue(writer, (T) value, serializer);
    }

    public sealed override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var isNullable = Nullable.GetUnderlyingType(objectType) != null;

        if (reader.TokenType == JsonToken.Null)
        {
            if (isNullable)
                return null;

            throw JsonConverterException.Create(
                reader,
                $"Property of type {typeof(T).Name} cannot be null.",
                GetType(),
                null
            );
        }

        try
        {
            var parsed = ParseValue(reader, objectType, existingValue, serializer);
            return isNullable ? (T?) parsed : parsed;
        }
        catch (Exception ex) when (ex is not JsonConverterException)
        {
            throw JsonConverterException.Create(
                reader,
                $"Error while converting value to {typeof(T).Name}. See inner exception.",
                GetType(),
                ex
            );
        }
    }

    protected abstract void WriteValue(JsonWriter writer, T value, JsonSerializer serializer);

    protected abstract T ParseValue(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer);
}