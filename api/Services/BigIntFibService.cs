using System.Numerics;

namespace Api.Services;

public class BigIntFibService : IFibService<BigInteger>
{
    private AppCacheService _cache;

    public BigIntFibService(AppCacheService cache)
    {
        _cache = cache;
    }

    public BigInteger RecursiveFib(int index)
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

    public BigInteger RecursiveFibWithCache(int index, bool cache = true)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        if (cache && _cache.Cache[typeof(BigInteger)].Count > index)
            return (BigInteger)_cache.Cache[typeof(BigInteger)][index];

        var sub2 = RecursiveFibWithCache(index - 2, cache);
        var sub1 = RecursiveFibWithCache(index - 1, cache);
        if (sub2 + sub1 < 0)
            throw new Exception("Error occured. Likely overflow.");

        var val = sub2 + sub1;
        if (cache)
            _cache.Cache[typeof(BigInteger)].Add(val);
        return val;
    }

    public BigInteger CancelableFib(int index, bool cache, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            throw new TaskCanceledException();
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        if (cache && _cache.Cache[typeof(BigInteger)].Count > index)
            return (BigInteger)_cache.Cache[typeof(BigInteger)][index];

        var sub2 = CancelableFib(index - 2, cache, token);
        var sub1 = CancelableFib(index - 1, cache, token);
        if (sub2 + sub1 < 0)
            throw new Exception("Error occured. Likely overflow.");

        var val = sub2 + sub1;
        if (cache)
            _cache.Cache[typeof(BigInteger)].Add(val);
        return val;
    }

    public List<BigInteger> ListFibWithCache(
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
