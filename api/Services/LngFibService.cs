using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace Api.Services;

public class LngFibService : IFibService<long>
{
    public List<long> _cache = new List<long> { };

    public LngFibService() { }

    public long RecursiveFib(int index)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        return RecursiveFib(index - 2) + RecursiveFib(index - 1);
    }

    public long RecursiveFibWithCache(int index, bool cache = true)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        if (cache && _cache.Count > index)
        {
            return _cache[index];
        }
        var val = RecursiveFibWithCache(index - 2, cache) + RecursiveFibWithCache(index - 1, cache);
        if (cache)
            _cache.Add(val);
        return val;
    }

    public List<long> ListFibWithCache(int startIndex, int endIndex, bool cache = true)
    {
        return Enumerable
            .Range(startIndex, endIndex)
            .AsParallel()
            .AsOrdered()
            .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
            .Select(n => RecursiveFibWithCache(n, cache))
            .ToList();
    }
}
