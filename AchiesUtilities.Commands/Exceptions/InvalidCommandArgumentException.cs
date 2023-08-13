using System.Runtime.Serialization;

namespace AchiesUtilities.Commands.Exceptions;

[Serializable]
public class InvalidCommandArgumentException : Exception
{
    public string ArgumentName { get; }
    public string? ArgumentValue { get; }

    public InvalidCommandArgumentException(string argumentName, string? argumentValue) : base($"Provided argument {argumentName} is invalid")
    {
        ArgumentName = argumentName;
        ArgumentValue = argumentValue;
    }

    public InvalidCommandArgumentException(string argumentName, string? argumentValue, string message) : base(message)
    {
        ArgumentName = argumentName;
        ArgumentValue = argumentValue;
    }

    public InvalidCommandArgumentException(string argumentName, string? argumentValue, string? message, Exception? inner) 
        : base(message, inner)
    {
        ArgumentName = argumentName;
        ArgumentValue = argumentValue;
    }

    protected InvalidCommandArgumentException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}