using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using AchiesUtilities.Commands.Core;

namespace AchiesUtilities.Commands;

public class StringCommand : IStringCommand
{
    public string CommandName { get; }
    public IList<ICommandArgument> Arguments => NamedArguments.Values.ToList();
 
    protected IReadOnlyDictionary<string, ICommandArgument> NamedArguments { get; set; }


    public StringCommand(string commandName)
    {
        CommandName = commandName;
        NamedArguments = new ReadOnlyDictionary<string, ICommandArgument>(new Dictionary<string, ICommandArgument>());
    }

    public StringCommand(string commandName, IEnumerable<ICommandArgument> arguments)
    {
        CommandName = commandName;
        NamedArguments = arguments.ToDictionary(a => a.Name, a => a);
    }

    protected void SetArguments(IEnumerable<ICommandArgument> arguments)
    {
        NamedArguments = arguments.ToDictionary(a => a.Name, a => a);
    }

    protected T GetArgument<T>([CallerMemberName] string name = "")
    {
        if (NamedArguments.TryGetValue(name, out var argument))
        {
            if (argument is not ICommandArgument<T> arg) 
                throw new InvalidOperationException($"Type mismatch: argument with name {name} is {argument.GetType().Name} instead of required {typeof(T).Name}");
            
            return arg.Value;
        }
        else
        {
            throw new InvalidOperationException($"Argument with name {name} was not found");
        }
    }
}