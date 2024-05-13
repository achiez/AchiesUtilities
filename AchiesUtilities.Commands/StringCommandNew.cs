using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AchiesUtilities.Commands.Core;
using Microsoft.VisualBasic;

namespace AchiesUtilities.Commands;

public class StringCommandNew
{


    public IList<INewCommandArgument> Arguments { get; }
    public IList<INamedCommandArgument<bool>> Flags { get; }


    public string CommandName { get; }
    public string Environment { get; }
    public string ArgumentsString { get; }

    private readonly Dictionary<string, object> _values = new();
    public StringCommandNew(string? environment, string commandName, IList<INewCommandArgument> arguments, IList<INamedCommandArgument<bool>> flags, string argumentsString)
    {
        Arguments = arguments;
        Flags = flags;
        ArgumentsString = argumentsString;
        CommandName = commandName;
        Environment = environment ?? string.Empty;
    }

    public T GetArgument<T>([CallerMemberName] string name = "") where T : notnull
    {
        if (_values.TryGetValue(name, out var value))
        {
            return (T)value;
        }
        else
        {
            var argument = Arguments.First(a => a is INamedCommandArgument na && na.Name == name);
            var val = (T) argument.ArgumentValue;
            _values[name] = val;
            return val;
        }
    }

    public T? TryGetArgument<T>([CallerMemberName] string name = "") where T : notnull
    {
        if (_values.TryGetValue(name, out var value))
        {
            return (T)value;
        }
        else
        {
            return default;
        }
    }

    public bool CheckArgument<T>(string name) where T : notnull
    {
        if (_values.TryGetValue(name, out var value))
        {
            return value is T;
        }
        else
        {
            var argument = Arguments.FirstOrDefault(a => a is INamedCommandArgument<T> na && na.Name == name);
            var valid = argument != null;
            if (valid)
            {
                _values[name] = ((INamedCommandArgument<T>)argument!).Value;
            }
            return valid;
        }
    }

    public static bool TryParse(string command, [NotNullWhen(returnValue: true)] out StringCommandNew? result)
    {
        return ParseInternal(command, false, true, out result);
    }

    public static StringCommandNew Parse(string command)
    {
        if (ParseInternal(command, false, false, out var result))
        {
            return result;
        }
        else
        {
            throw new FormatException("Command is not valid");
        }
    }

    private static bool ParseInternal(string command, bool parseEnvironment, bool trying, [NotNullWhen(returnValue: true)] out StringCommandNew? result)
    {
        var split = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        result = null;
        if (string.IsNullOrWhiteSpace(command) || split.Length == 0)
        {
            if (trying)
                return false;
            throw new FormatException("Command is empty");
        }

        string environment = "";
        if (parseEnvironment)
        {
            environment = split[0];
            if (CheckIsArgumentName(environment))
            {
                if (trying) return false;
                throw new FormatException("Environment is not valid. It can't start with '-'");
            }

            split = split.Skip(1).ToArray();
        }

        var commandName = split[0];
        if (CheckIsArgumentName(commandName))
        {
            if(trying) return false;
            throw new FormatException("Command name is not valid. It can't start with '-'");
        }

        split = split.Skip(1).ToArray();

        var arguments = new List<INewCommandArgument>();
        var flags = new List<INamedCommandArgument<bool>>();
        for (var i = 0; i < split.Length; i++)
        {
            var arg1 = split[i];
            string? arg2 = null;
            if (i < split.Length - 1)
            {
                arg2 = split[i + 1];
            }
            var argument = ParseArgument(arg1, arg2, out var isFlag, out var isNamed);
            arguments.Add(argument);
            if (isFlag)
            {
                flags.Add((INamedCommandArgument<bool>)argument);
            }

            if (isNamed)
            {
                i++;
            }
        }

        var argString = string.Join(' ', split);
        result  = new StringCommandNew(environment, commandName, arguments, flags, argString);
        return true;
    }


    private static bool CheckIsArgumentName(string str)
    {
        return str.StartsWith("--") || str.StartsWith("-");
    }

    private static string ExtractName(string str)
    {
        return str.TrimStart('-');
    }


    private static NewCommandArgument ParseArgument(string argument1, string? argument2, out bool isFlag, out bool isNamed)
    {
        isFlag = CheckIsArgumentName(argument1) && (argument2 == null || CheckIsArgumentName(argument2));
        isNamed = CheckIsArgumentName(argument1) && (argument2 == null || !CheckIsArgumentName(argument2));

        if (isNamed)
        {
            var name = ExtractName(argument1);
            if (argument2 == null)
            {
                return new NewCommandArgument<bool>(true).AsNamed(name);
            }
            else
            {
                var value = argument2;
                if (value == null)
                {
                    throw new FormatException("Value is not valid");
                }

                var intVal = TryParseInt(value);
                if (intVal != null)
                {
                    return intVal.AsNamed(name);
                }

                var boolVal = TryParseBool(value);
                if (boolVal != null)
                {
                    return boolVal.AsNamed(name);
                }

                return new NewCommandArgument<string>(value).AsNamed(name);
            }
        }
        else
        {
            var value = argument1;
            if (value == null)
            {
                throw new FormatException("Value is not valid");
            }

            var intVal = TryParseInt(value);
            if (intVal != null)
            {
                return intVal;
            }

            var boolVal = TryParseBool(value);
            if (boolVal != null)
            {
                return boolVal;
            }

            return new NewCommandArgument<string>(value);
        }

    }

    private static NewCommandArgument<int>? TryParseInt(string str)
    {
        if (int.TryParse(str, out var result))
        {
            return new NewCommandArgument<int>(result);
        }
        else
        {
            return null;
        }
    }

    private static NewCommandArgument<bool>? TryParseBool(string str)
    {
        if (bool.TryParse(str, out var result))
        {
            return new NewCommandArgument<bool>(result);
        }
        else
        {
            return null;
        }
    }
}

public interface INewCommandArgument
{
    public object ArgumentValue { get; set; }

}


public interface INewCommandArgument<T> : INewCommandArgument where T : notnull
{
    public T Value
    {
        get => (T)ArgumentValue;
        set => ArgumentValue = value;
    }
}

public interface INamedCommandArgument : INewCommandArgument
{
    public string Name { get; }
}

public interface INamedCommandArgument<T> : INewCommandArgument<T>, INamedCommandArgument where T : notnull
{
 
}

public class NewCommandArgument : INewCommandArgument
{
    public NewCommandArgument(object argumentValue)
    {
        ArgumentValue = argumentValue;
    }

    public object ArgumentValue { get; set; }

    public NewNamedCommandArgument<T> AsNamed<T>(string name) where T : notnull
    {
        return new NewNamedCommandArgument<T>(name, (T)ArgumentValue);
    }
}

public class NewNamedCommandArgument : NewCommandArgument, INamedCommandArgument
{
    public string Name { get; }


    public NewNamedCommandArgument(string name, object argumentValue) : base(argumentValue)
    {
        Name = name;
    }
}

public class NewCommandArgument<T> : NewCommandArgument, INewCommandArgument<T> where T : notnull
{
    public T Value
    {
        get => (T)ArgumentValue;
        set => ArgumentValue = value;
    }

    public NewCommandArgument(T argumentValue) : base(argumentValue)
    {
    }

    public NewNamedCommandArgument<T> AsNamed(string name)
    {
        return new NewNamedCommandArgument<T>(name, Value);
    }
}

public class NewNamedCommandArgument<T> : NewNamedCommandArgument, INamedCommandArgument<T> where T : notnull
{
    public T Value
    {
        get => (T)ArgumentValue;
        set => ArgumentValue = value;
    }

    public NewNamedCommandArgument(string name, T argumentValue) : base(name, argumentValue)
    {
    }
}