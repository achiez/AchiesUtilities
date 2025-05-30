namespace AchiesUtilities.Caching;

[Obsolete]
public abstract class JsonCache<TKey, TValue, TCache>
{
    /*
    private void LogAdded(AchiesUtilities.CachedItem<TValue> item)
    {
        if (FullTracingEnabled)
        {
            Logger.Trace("Объект добавлен в кеш\n{@value} ", item);
        }
        else
        {
            Logger.Debug("Объект {value} добавлен в кеш", item.Value);
        }
    }

    private void LogRemoved(AchiesUtilities.CachedItem<TValue> item)
    {
        if (FullTracingEnabled)
            Logger.Trace("Объект удален из кеша\n{@value}", item);
        else
            Logger.Debug("Объект {value} удален из кеша", item.Value);
    }

    private void LogSetExpired(AchiesUtilities.CachedItem<TValue> item)
    {
        if (FullTracingEnabled)
            Logger.Trace("Объект вручную установлен на истекшим\n{@value}", item);
        else
            Logger.Trace("Объект {value} вручную установлен истекшим", item.Value);
    }

    private void LogExpired(AchiesUtilities.CachedItem<TValue> item)
    {
        if (FullTracingEnabled)
            Logger.Trace("У объекта истек срок действия\n{@value}", item);
        else
            Logger.Trace("Объект {value} кешированный {time} истек", item.Value, item.CachedAt);
    }

    private void LogSaved()
    {
        if (FullTracingEnabled)
            Logger.Trace("Кеш сохранен. Объектов: {count}", Data.Cache.Count);
        else if (TimerSaveDisabled == false)
            Logger.Trace("Кеш сохранен");
    }
    */
}