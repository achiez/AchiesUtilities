using Newtonsoft.Json;
using System.Runtime.Serialization;

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

    public JsonConverterException(string message, string path, int lineNumber, int linePosition, Type? converterType, Exception? innerException) : base(message, path, lineNumber, linePosition, innerException)
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
        message = FormatMessage(lineInfo, reader.Path, converterType, message);

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

    private static string FormatMessage(IJsonLineInfo? lineInfo, string path, Type? converterType, string message)
    {
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
}
