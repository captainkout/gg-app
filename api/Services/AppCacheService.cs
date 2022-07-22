using System.Numerics;

namespace Api.Services;

public class AppCacheService
{
    public DateTime Birth;
    public Dictionary<Type, List<object>> Cache = new();

    public AppCacheService()
    {
        Reset();
    }

    public void Reset()
    {
        Cache = new()
        {
            { typeof(int), new List<object>() },
            { typeof(long), new List<object>() },
            { typeof(BigInteger), new List<object>() }
        };
        Birth = DateTime.Now;
    }
}
