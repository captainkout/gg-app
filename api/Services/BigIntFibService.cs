using System.Numerics;

namespace Api.Services;

public class BigIntFibService : IFibService<BigInteger>
{
    public List<BigInteger> _cache = new();

    public BigIntFibService() { }

    public BigInteger RecursiveFib(int index)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        return RecursiveFib(index - 2) + RecursiveFib(index - 1);
    }

    public BigInteger RecursiveFibWithCache(int index, bool cache = true)
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
}
