namespace AchiesUtilities.Commands.Templates;

//public abstract class RegexCommandTemplate<TCommand> : ICommandTemplate<TCommand> where TCommand : IStringCommand
//{
//    public bool IgnoreCase => Regex.Options.HasFlag(RegexOptions.IgnoreCase);

//    public Regex Regex { get; }

//    protected RegexCommandTemplate(Regex regex)
//    {
//        Regex = regex;
//    }

//    protected StringCommand? Deconstruct(string command)
//    {
//        var match = Regex.Match(command);
//        if (!match.Success) return null;

//        var arguments = 
//            match.Groups.Values.Select(group => new CommandArgument<string>(group.Name, group.Value)).ToList();

//        return new StringCommand(arguments);
//    }

//    public abstract TCommand Parse(string? command);
//    public abstract bool TryParse(string? command, [NotNullWhen(returnValue: true)] out TCommand? value);
//}