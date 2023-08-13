namespace AchiesUtilities.Commands.Core;

public interface ICommandArgument
{
    public string Name { get; }
    public object? ArgumentValue { get; set; }
}


public interface ICommandArgument<T> : ICommandArgument
{
    public T Value
    {
        get => (T)ArgumentValue;
        set => ArgumentValue = value;
    }
}