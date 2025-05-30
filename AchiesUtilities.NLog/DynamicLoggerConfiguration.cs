using NLog;

namespace AchiesUtilities.NLog;

public abstract class DynamicLoggerConfiguration
{
    public Type ParameterType { get; }

    protected DynamicLoggerConfiguration(Type parameterType)
    {
        ParameterType = parameterType;
    }

    public abstract void ConfigureLogger<T>(T parameter, Logger logger);
}

public delegate void DynamicLoggerConfigurationDelegate<in T>(T parameter, Logger logger);

public class DynamicLoggerConfiguration<TParam> : DynamicLoggerConfiguration
{
    public DynamicLoggerConfigurationDelegate<TParam> ConfigurationDelegate { get; }


    public DynamicLoggerConfiguration(DynamicLoggerConfigurationDelegate<TParam> configurationDelegate) : base(
        typeof(TParam))
    {
        ConfigurationDelegate = configurationDelegate;
    }

    public override void ConfigureLogger<T>(T parameter, Logger logger)
    {
        if (parameter is not TParam param)
        {
            throw new InvalidOperationException("Invalid type");
        }

        ConfigurationDelegate(param, logger);
    }
}