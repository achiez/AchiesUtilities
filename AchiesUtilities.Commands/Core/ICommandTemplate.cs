using System.Diagnostics.CodeAnalysis;

namespace AchiesUtilities.Commands.Core;

public interface ICommandTemplate
{
    public IStringCommand ParseCommand(string command, object? context = null);
    public bool Check(string command, object? context = null);

}

public interface ICommandTemplate<out TCommand> : ICommandTemplate where TCommand : IStringCommand
{
    public TCommand Parse(string command, object? context = null);
    IStringCommand ICommandTemplate.ParseCommand(string command, object? context) => Parse(command, context);

}