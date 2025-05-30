using Newtonsoft.Json;

namespace AchiesUtilities.Config;

public abstract class Config
{
    private static JsonSerializerSettings? _settings;
    private static Dictionary<Type, Config> Instances;
    private static bool _initialized;

    public static void Initialize(JsonSerializerSettings settings)
    {
        if (_initialized) return;
        _initialized = true;
        _settings = settings;
    }
}