using AchiesUtilities.Newtonsoft.JSON.Converters.Common;
using AchiesUtilities.Newtonsoft.JSON.Converters.Special;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON;

public static class StaticConverters
{
    /// <summary>
    ///     Include all defaults instances of <see cref="JsonConverter" />:<br />
    ///     <see cref="TimeSpanConverter" /><br /><see cref="UnixTimeStampConverterSeconds" />
    /// </summary>
    public static IList<JsonConverter> AllDefaultConverters { get; } = new JsonConverter[]
    {
        TimeSpanConverter,
        UnixTimeStampConverterSeconds,
        NullableUnixTimeStampConverterSeconds
    };

    #region Default

    public static JsonTimespanConverter TimeSpanConverter { get; } = new();
    public static UnixTimeStampConverter UnixTimeStampConverterSeconds { get; } = new();
    public static NullableUnixTimeStampConverter NullableUnixTimeStampConverterSeconds { get; } = new();

    #endregion

    #region Special

    public static BoolToIntConverter BoolToIntConverter { get; } = new();
    public static JsonTimeSpanTypedConverter TimeSpanTypedConverter { get; } = new();

    #endregion
}