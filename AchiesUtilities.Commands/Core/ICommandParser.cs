namespace AchiesUtilities.Commands.Core;

public interface ICommandParser
{
    public IStringCommand? ParseCommand(string command);

    public bool IsValid(string command);
}

public interface ICommandParser<TCommand> : ICommandParser where TCommand : IStringCommand
{
    public TCommand? Parse(string command);

    IStringCommand? ICommandParser.ParseCommand(string command)
    {
        return Parse(command);
    }
}