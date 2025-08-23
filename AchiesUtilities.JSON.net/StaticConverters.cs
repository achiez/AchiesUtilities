using AchiesUtilities.Newtonsoft.JSON.Converters.Common;
using AchiesUtilities.Newtonsoft.JSON.Converters.Special;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Newtonsoft.JSON;

[PublicAPI]
public static class StaticConverters
{
    /// <summary>
    ///     Include all defaults instances of <see cref="JsonConverter" />:<br />
    ///     <see cref="JsonTimespanConverter" /><br />
    ///     <see cref="Converters.Special.UnixTimeStampConverter" /> (seconds)<br />
    ///     <see cref="Converters.Common.IPAddressConverter" /><br />
    ///     <see cref="Converters.Common.IPEndPointConverter" /><br />
    /// </summary>
    public static IList<JsonConverter> AllDefaultConverters { get; }


    #region Special

    public static BoolToIntConverter BoolToIntConverter { get; } = new();

    #endregion

    static StaticConverters()
    {
        AllDefaultConverters =
        [
            TimeSpanConverter,
            UnixTimeStampConverterSeconds,
            IPAddressConverter,
            IPEndPointConverter
        ];
    }

    #region Default

    public static JsonTimespanConverter TimeSpanConverter { get; } = new();
    public static UnixTimeStampConverter UnixTimeStampConverterSeconds { get; } = new();
    public static IPAddressConverter IPAddressConverter { get; } = new();
    public static IPEndPointConverter IPEndPointConverter { get; } = new();

    #endregion
}