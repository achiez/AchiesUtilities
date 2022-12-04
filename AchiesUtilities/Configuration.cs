namespace AchiesUtilities;

public static class Configuration
{
    /// <summary>
    /// Default value - <b>100</b> ms
    /// </summary>
    public static readonly SetOnceProperty<int> DefaultPauseTokenDelay = new(100);

}