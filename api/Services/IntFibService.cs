namespace api.Services;

public class IntFibService : IFibService<int>
{
    public List<int> _cache = new();

    public IntFibService() { }

    public int RecursiveFib(int index)
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

    public int RecursiveFibWithCache(int index, bool cache = true)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        if (cache && _cache.Count > index)
            return _cache[index];

        var sub2 = RecursiveFibWithCache(index - 2, cache);
        var sub1 = RecursiveFibWithCache(index - 1, cache);
        if (sub2 + sub1 < 0)
            throw new Exception("Error occured. Likely overflow.");

        var val = sub2 + sub1;
        if (cache)
            _cache.Add(val);
        return val;
    }
}
