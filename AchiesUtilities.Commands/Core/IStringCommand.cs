namespace AchiesUtilities.Commands.Core;

public interface IStringCommand
{
    public IList<ICommandArgument> Arguments { get; }

    public string CommandName { get; }
}