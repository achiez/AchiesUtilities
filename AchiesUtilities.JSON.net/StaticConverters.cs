using AchiesUtilities.Newtonsoft.JSON.Converters.Default;
using AchiesUtilities.Newtonsoft.JSON.Converters.Special;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON;

[PublicAPI]
public static class StaticConverters
{
    #region Default

    public static JsonTimespanConverter TimeSpanConverter { get; } = new();
    public static UnixTimeStampConverter UnixTimeStampConverterSeconds { get; } = new();

    #endregion

    #region Special

    public static IntToBoolConverter IntToBoolConverter { get; } = new();
    public static JsonTimeSpanTypedConverter TimeSpanTypedConverter { get; } = new();
    #endregion

    /// <summary>
    /// Include all defaults instances of <see cref="JsonConverter"/>:<br/>
    /// <see cref="TimeSpanConverter"/><br/><see cref="UnixTimeStampConverterSeconds"/>
    /// </summary>
    public static IList<JsonConverter> AllDefaultConverters { get; } = new JsonConverter[]
    {
        TimeSpanConverter,
        UnixTimeStampConverterSeconds,
    };

}