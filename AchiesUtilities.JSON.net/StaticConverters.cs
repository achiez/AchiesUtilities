using AchiesUtilities.Newtonsoft.JSON.Converters.Common;
using AchiesUtilities.Newtonsoft.JSON.Converters.Special;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON;

public static class StaticConverters
{
    /// <summary>
    ///     Include all defaults instances of <see cref="JsonConverter" />:<br />
    ///     <see cref="TimeSpanConverter" /><br /><see cref="UnixTimeStampConverterSeconds" /><br/><see cref="NullableUnixTimeStampConverterSeconds"/>
    /// </summary>
    public static IList<JsonConverter> AllDefaultConverters { get; }

    #region Default

    public static JsonTimespanConverter TimeSpanConverter { get; } = new();
    public static UnixTimeStampConverter UnixTimeStampConverterSeconds { get; } = new();
    public static NullableUnixTimeStampConverter NullableUnixTimeStampConverterSeconds { get; } = new();
    public static IPAddressConverter IPAddressConverter { get; } = new();
    public static IPEndPointConverter IPEndPointConverter { get; } = new();

    #endregion


    #region Special

    public static BoolToIntConverter BoolToIntConverter { get; } = new();
    public static JsonTimeSpanTypedConverter TimeSpanTypedConverter { get; } = new();

    #endregion

    static StaticConverters()
    {
        AllDefaultConverters = 
        [
            TimeSpanConverter,
            UnixTimeStampConverterSeconds,
            NullableUnixTimeStampConverterSeconds,
            IPAddressConverter,
            IPEndPointConverter
        ];
    }
}