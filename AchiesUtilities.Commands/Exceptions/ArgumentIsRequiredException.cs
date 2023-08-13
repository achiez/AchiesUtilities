using System.Runtime.Serialization;

namespace AchiesUtilities.Commands.Exceptions;

public class CommandArgumentIsRequiredException : InvalidCommandArgumentException
{
    public CommandArgumentIsRequiredException(string argumentName) : base(argumentName, 
        null, $"Argument {argumentName} is required")
    {
    }

    public CommandArgumentIsRequiredException(string argumentName,  string message) 
        : base( argumentName, null, message)
    {
    }

    public CommandArgumentIsRequiredException(string argumentName, string? message, Exception? inner) 
        : base( argumentName, null, message, inner)
    {
    }

    protected CommandArgumentIsRequiredException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}