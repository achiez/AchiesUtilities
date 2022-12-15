using JetBrains.Annotations;

namespace AchiesUtilities;

[PublicAPI]
public class PauseToken
{
    /// <summary>
    /// Delay before each check
    /// </summary>
    public int MsDelay { get; init; } = Configuration.DefaultPauseTokenDelay.Value;
    public bool IsPaused { get; private set; }
    public void AwaitPause(CancellationToken cancellationToken, Action? onPauseAction = null, Action? onPauseEnds = null)
    {
        var everPaused = false;
        while (IsPaused)
        {
            everPaused = true;
            if (cancellationToken.IsCancellationRequested) return;
            if (onPauseAction != null)
            {
                onPauseAction.Invoke();
                onPauseAction = null;
            }
            if (cancellationToken.IsCancellationRequested) return;
            Thread.Sleep(MsDelay);
        }
        if (everPaused)
            onPauseEnds?.Invoke();
    }

    public async Task AwaitPauseAsync(CancellationToken cancellationToken, Func<Task>? onPauseAction = null, Func<Task>? onPauseEnds = null)
    {
        var everPaused = false;
        while (IsPaused)
        {
            everPaused = true;
            if (cancellationToken.IsCancellationRequested) return;
            if (onPauseAction != null)
            {
                await onPauseAction();
                onPauseAction = null;
            }
            if (cancellationToken.IsCancellationRequested) return;
            await Task.Delay(MsDelay, cancellationToken);
        }

        if (everPaused && onPauseEnds != null)
            await onPauseEnds();
    }

    public void Pause()
    {
        IsPaused = true;
    }
    public void Reset()
    {
        IsPaused = false;
    }
}