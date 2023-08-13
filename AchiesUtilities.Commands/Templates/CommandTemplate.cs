using System.Diagnostics.CodeAnalysis;
using AchiesUtilities.Commands.Core;

namespace AchiesUtilities.Commands.Templates;

public abstract class CommandTemplate<TCommand> : ICommandTemplate<TCommand> where TCommand : IStringCommand
{
    public abstract bool IgnoreCase { get; set; }
    public abstract bool Check(string command, object? context = null);
    public abstract TCommand Parse(string command, object? context = null);

}