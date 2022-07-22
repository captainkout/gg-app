using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using Api.Models;

namespace Api.Services;

public class AppStateService
{
    public AppState State = new();
    private readonly AppCacheService _cache;
    private readonly IConfiguration _config;

    private long StartupMem;
    public ConcurrentDictionary<CancellationTokenSource, bool> Queue = new();

    public AppStateService(AppCacheService cache, IConfiguration config)
    {
        _cache = cache;
        _config = config;
        StartupMem = Process.GetCurrentProcess().PrivateMemorySize64 / 1000;
    }

    /// <summary>
    /// Check Cache Age/Size => Clear if necesary
    /// Check App Memory => Start Canceling tasks
    /// </summary>
    public void UpdateAppState()
    {
        var cacheAge = (int)(DateTime.Now - _cache.Birth).TotalMilliseconds;
        if (cacheAge > _config.GetSection("AppState").GetValue<int>("MaxCacheAge_ms"))
            _cache.Reset();

        var appMem = Process.GetCurrentProcess().PrivateMemorySize64 / 1000 - StartupMem;
        while (
            appMem > _config.GetSection("AppState").GetValue<int>("MaxAdditionalMemory_kb")
            && Queue.Count > 0
        )
        {
            var tokenSource = Queue.First();
            Queue.TryRemove(tokenSource);
            tokenSource.Key.Cancel();

            _cache.Reset();

            appMem = Process.GetCurrentProcess().PrivateMemorySize64 / 1000 - StartupMem;
        }

        State = new AppState()
        {
            CacheCnt =
                _cache.Cache[typeof(int)].Count
                + _cache.Cache[typeof(long)].Count
                + _cache.Cache[typeof(BigInteger)].Count,
            CacheAge = cacheAge,
            MemoryUsage = appMem,
            QueueLength = Queue.Count
        };
    }
}
