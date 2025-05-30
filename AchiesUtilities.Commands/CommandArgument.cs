using AchiesUtilities.Commands.Core;

namespace AchiesUtilities.Commands;

public class CommandArgument<T> : ICommandArgument<T>
{
    public bool IsRequired { get; set; }
    public object? ArgumentValue { get; set; }
    public string Name { get; }

    public CommandArgument(string name, T argumentValue, bool isRequired = false)
    {
        Name = name;
        ArgumentValue = argumentValue;
        IsRequired = isRequired;
    }
}