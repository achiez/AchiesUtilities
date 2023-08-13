using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AchiesUtilities.Config;

public abstract class Config
{
    private static JsonSerializerSettings? _settings;
    private static Dictionary<Type, Config> Instances;
    private static bool _initialized;
    
    public static void Initialize(JsonSerializerSettings settings)
    {
        if(_initialized) return;
        _initialized = true;
        _settings = settings;
    }





    

}