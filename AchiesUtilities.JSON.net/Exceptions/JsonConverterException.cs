using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON.Exceptions;

public class JsonConverterException : JsonSerializationException
{
    public Type? ConverterType { get; }
    public string? ConverterName => ConverterType?.Name;

    public JsonConverterException()
    {
    }

    public JsonConverterException(string message) : base(message)
    {
    }

    public JsonConverterException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public JsonConverterException(string message, string path, int lineNumber, int linePosition, Type? converterType,
        Exception? innerException) : base(message, path, lineNumber, linePosition, innerException)
    {
        ConverterType = converterType;
    }

    protected JsonConverterException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }

    public static JsonConverterException Create(JsonReader reader, string message, Type? converterType, Exception? ex)
    {
        var lineInfo = reader as IJsonLineInfo;
        message = FormatMessage(reader, reader.Path, converterType, message);

        int lineNumber;
        int linePosition;
        if (lineInfo != null && lineInfo.HasLineInfo())
        {
            lineNumber = lineInfo.LineNumber;
            linePosition = lineInfo.LinePosition;
        }
        else
        {
            lineNumber = 0;
            linePosition = 0;
        }

        return new JsonConverterException(message, reader.Path, lineNumber, linePosition, converterType, ex);
    }

    private static string FormatMessage(JsonReader reader, string path, Type? converterType, string message)
    {
        var lineInfo = reader as IJsonLineInfo;
        // don't add a fullstop and space when message ends with a new line
        if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
        {
            message = message.Trim();

            if (!message.EndsWith('.'))
            {
                message += ".";
            }

            message += " ";
        }

        var value = GetValueString(reader);
        message += $"Provided value: '{value}'. ";
        message += $"Path '{path}'";

        if (lineInfo != null && lineInfo.HasLineInfo())
        {
            message += $", line {lineInfo.LineNumber}, position {lineInfo.LinePosition}.";
            if (converterType != null)
            {
                message += $"  Converter: {converterType}";
            }
        }

        message += ".";

        return message;
    }

    private static string GetValueString(JsonReader reader)
    {
        if (reader.Value != null) return reader.Value.ToString() ?? "null";
        var result = reader.TokenType switch
        {
            JsonToken.StartObject => "{object}",
            JsonToken.StartArray => "{array}",
            _ => null
        };
        result ??= $"{{{reader.TokenType}}}";
        return result;
    }
}