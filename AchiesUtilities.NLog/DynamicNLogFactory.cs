using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace AchiesUtilities.NLog;

[PublicAPI]
public class DynamicNLogFactory
{

    private readonly NLogLoggerProvider _provider;
    private readonly ConstructorInfo _iLoggerCtor;
    private readonly object[] _ctorArgs = new object[2];
    private readonly LogFactory _logFactory;
    private readonly Type _nLogLoggerType;
    private readonly FieldInfo _loggerField;

    private readonly IReadOnlyDictionary<Type, DynamicLoggerConfiguration> _configurations;
    private readonly ILoggerFactory _loggerFactory; //Default implementation

    public DynamicNLogFactory(ILoggerFactory loggerFactory) : this(loggerFactory, new DynamicNLogFactoryOptions())
    { }

    public DynamicNLogFactory(ILoggerFactory loggerFactory, DynamicNLogFactoryOptions factoryOptions)
    {
        _loggerFactory = loggerFactory;
        _configurations = 
            new ReadOnlyDictionary<Type, DynamicLoggerConfiguration>(
                factoryOptions.Configurations.ToDictionary(x => x.ParameterType, x => x));

        if (loggerFactory is not LoggerFactory f)
        {
            throw new InvalidOperationException("LoggerFactory must be of type NLogLoggerFactory");
        }

        NLogProviderOptions? options = null;
        object? beginScopeParser = null;
        try
        {

            var regField = f.GetType()
                .GetField("_providerRegistrations", BindingFlags.NonPublic | BindingFlags.Instance);

            var enumerableGenericType = regField!.FieldType.GetGenericArguments()[0];
            var val = regField.GetValue(f) as IEnumerable;
            var providerField = enumerableGenericType.GetField("Provider", BindingFlags.Public | BindingFlags.Instance);
            foreach (var providerRegistration in val!)
            {
                var provider = providerField!.GetValue(providerRegistration);
                if (provider is NLogLoggerProvider p)
                {
                    var parserField = p.GetType().GetField("_beginScopeParser", BindingFlags.NonPublic | BindingFlags.Instance);
                    _provider = p;
                    options = p.Options;
                    beginScopeParser = parserField!.GetValue(p)!;
                    _logFactory = p.LogFactory;
                    break;
                }

            }
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Can't get NLogLoggerProvider in current services, see inner exception",
                e);
        }

        if (_provider == null)
            throw new InvalidOperationException("Can't find NLoggerProvider in current logger providers");

        if(options == null)
            throw new InvalidOperationException("Can't find NLogProviderOptions in NLogLoggerProvider");

        if (beginScopeParser == null)
            throw new InvalidOperationException("Can't find _beginScopeParser in NLogLoggerProvider");

        if(_logFactory == null)
            throw new InvalidOperationException("Can't find LogFactory in NLogLoggerProvider");

        _nLogLoggerType = Assembly.Load("NLog.Extensions.Logging").GetType("NLog.Extensions.Logging.NLogLogger")!;
        if (_nLogLoggerType == null)
            throw new InvalidOperationException("Can't find NLogLogger type");

        _iLoggerCtor = _nLogLoggerType.GetConstructor(new[]{typeof(Logger), typeof(NLogProviderOptions), beginScopeParser.GetType()})!;
        if (_iLoggerCtor == null)
            throw new InvalidOperationException("Can't find NLogLogger constructor");

        _loggerField = _nLogLoggerType.GetField("_logger", BindingFlags.NonPublic | BindingFlags.Instance)!;
        if (_loggerField == null)
            throw new InvalidOperationException("Can't find _logger field in NLogLogger");
    }


    public void Dispose()
    {
        _provider.Dispose();
    }
    public ILogger<T> CreateDynamicLogger<T>(T parameter, string name) where T : notnull
    {
        return CreateDynamicLogger<T, T>(parameter, name);
    }

    public ILogger<TLogger> CreateDynamicLogger<T, TLogger>(T parameter, string name) where T : notnull where TLogger : notnull
    {
        ArgumentNullException.ThrowIfNull(parameter);
        var parameterType = typeof(T);
        var logger = _logFactory.GetLogger(name);
        foreach (var (type, config) in _configurations)
        {
            if (parameterType.IsAssignableFrom(type))
            {
                config.ConfigureLogger(parameter, logger);
            }
        }
        var iLogger = (ILogger)InvokeCtor(logger);
        return new Logger<TLogger>(new OneTimeFactory(iLogger));
    }



    public ILogger<TLogger> CreateWithExactConfiguration<T, TLogger>(T parameter, string name) where T : notnull where TLogger : notnull
    {
        ArgumentNullException.ThrowIfNull(parameter);
        var logger = _logFactory.GetLogger(name);
        if (_configurations.TryGetValue(typeof(T), out var configuration))
        {
            configuration.ConfigureLogger(parameter, logger);
        }
        var iLogger = (ILogger)InvokeCtor(logger);
        return new Logger<TLogger>(new OneTimeFactory(iLogger));
    }


    private object InvokeCtor(Logger logger)
    {
        return _iLoggerCtor.Invoke(new[] {logger, _ctorArgs[0], _ctorArgs[1]});
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerFactory.CreateLogger(categoryName);
    }

    public void AddProvider(ILoggerProvider provider)
    {
        _loggerFactory.AddProvider(provider);
    }


    public bool SetLoggerProperty<T>(ILogger logger, string name, T parameter)
    {
        if(logger.GetType() != _nLogLoggerType)
            return false;

        var nLogger = (Logger)_loggerField.GetValue(logger)!;
        nLogger.Properties[name] = parameter;
        return true;
    }

    private sealed class OneTimeFactory(ILogger logger) : ILoggerFactory
    {
        public ILogger CreateLogger(string categoryName)
        {
            return logger;
        }


        public void AddProvider(ILoggerProvider provider){} //Ignored
        public void Dispose(){} //Ignored
    }
}