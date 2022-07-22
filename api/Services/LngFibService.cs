using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace Api.Services;

public class LngFibService : IFibService<long>
{
    private AppCacheService _cache;

    public LngFibService(AppCacheService cache)
    {
        _cache = cache;
    }

    public long RecursiveFib(int index)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;

        var sub2 = RecursiveFib(index - 2);
        var sub1 = RecursiveFib(index - 1);
        if (sub2 + sub1 < 0)
            throw new Exception("Error occured. Likely overflow.");

        return sub2 + sub1;
    }

    public long RecursiveFibWithCache(int index, bool cache = true)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        if (cache && _cache.Cache[typeof(long)].Count > index)
            return (long)_cache.Cache[typeof(long)][index];

        var sub2 = RecursiveFibWithCache(index - 2, cache);
        var sub1 = RecursiveFibWithCache(index - 1, cache);
        if (sub2 + sub1 < 0)
            throw new Exception("Error occured. Likely overflow.");

        var val = sub2 + sub1;
        if (cache)
            _cache.Cache[typeof(long)].Add(val);
        return val;
    }

    public long CancelableFib(int index, bool cache, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            throw new TaskCanceledException();
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        if (cache && _cache.Cache[typeof(long)].Count > index)
            return (long)_cache.Cache[typeof(long)][index];

        var sub2 = CancelableFib(index - 2, cache, token);
        var sub1 = CancelableFib(index - 1, cache, token);
        if (sub2 + sub1 < 0)
            throw new Exception("Error occured. Likely overflow.");

        var val = sub2 + sub1;
        if (cache)
            _cache.Cache[typeof(long)].Add(val);
        return val;
    }

    public List<long> ListFibWithCache(
        int startIndex,
        int endIndex,
        bool cache,
        CancellationToken token
    )
    {
        return Enumerable
            .Range(startIndex, endIndex)
            .AsParallel()
            .AsOrdered()
            .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
            .Select(n => CancelableFib(n, cache, token))
            .ToList();
    }
}
