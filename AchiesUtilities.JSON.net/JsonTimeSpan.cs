﻿namespace AchiesUtilities.Newtonsoft.JSON;

public readonly struct JsonTimeSpan
{
    public long Ticks { get; }

    public JsonTimeSpan(long ticks)
    {
        Ticks = ticks;
    }

    public TimeSpan ToTimeSpan()
    {
        return new TimeSpan(Ticks);
    }

    public static JsonTimeSpan FromTimeSpan(TimeSpan timeSpan)
    {
        return new JsonTimeSpan(timeSpan.Ticks);
    }

    public static implicit operator TimeSpan(JsonTimeSpan value)
    {
        return value.ToTimeSpan();
    }

    public static implicit operator JsonTimeSpan(TimeSpan value)
    {
        return FromTimeSpan(value);
    }
}