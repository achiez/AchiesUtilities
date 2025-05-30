using System.Runtime.Serialization;

namespace AchiesUtilities.Commands.Exceptions;

[Serializable]
public class InvalidCommandException : Exception
{
    public string? ProvidedString { get; }

    public InvalidCommandException(string? providedString) : base("Provided string is not valid command")
    {
        ProvidedString = providedString;
    }

    public InvalidCommandException(string? providedString, string? message) : base(message)
    {
        ProvidedString = providedString;
    }

    public InvalidCommandException(string? providedString, string? message, Exception? inner) : base(message, inner)
    {
        ProvidedString = providedString;
    }

    protected InvalidCommandException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}