using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace AchiesUtilities.AsyncHelpers;

[PublicAPI]
public class BackgroundTaskChecker : IAsyncDisposable
{
    private readonly ConcurrentQueue<Task> _tasks = new();
    private readonly CancellationTokenSource _cts = new();
    private Task? _checkTask;
    private readonly TimeSpan _downtime;
    private bool _disposed;
    public BackgroundTaskChecker(TimeSpan downtime)
    {
        _downtime = downtime;
    }

    public void BeginCheck()
    {
        ThrowIfDisposed();
        if (_checkTask == null)
            _checkTask = Task.Run(Work);
        else
            throw new InvalidOperationException("Check already started");
    }

    private async Task Work()
    {
        while (_cts.IsCancellationRequested == false)
        {
            if (_tasks.TryDequeue(out var task))
            {
                if (task.IsCompleted)
                    await task;
                else
                    _tasks.Enqueue(task);
            }
            else
            {
                try
                {
                    await Task.Delay(_downtime, _cts.Token);
                }
                catch (TaskCanceledException) { }
            }
        }
    }

    public void AddTask(Task task)
    {
        ThrowIfDisposed();
        _tasks.Enqueue(task);
    }
    public async ValueTask Check()
    {
        ThrowIfDisposed();
        if (_checkTask is { IsCompleted: true })
        {
            await _checkTask;
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(BackgroundTaskChecker));
    }
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        _cts.Cancel();
        if (_checkTask != null)
        {
            await _checkTask;
            _checkTask = null;
        }

        await Task.WhenAll(_tasks);
    }
}