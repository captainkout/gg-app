using System.Numerics;
using Api.Extensions;
using Api.Services;
using Shouldly;
using Xunit;

namespace tests;

public class BigIntFibServiceTests
{
    private AppCacheService _cache;
    private IFibService<BigInteger> _fibService;

    public BigIntFibServiceTests()
    {
        _cache = new();
        _fibService = new BigIntFibService(_cache);
    }

    [Fact]
    public void RecursiveFib()
    {
        _fibService.RecursiveFib(0).ShouldBe(0);
        _fibService.RecursiveFib(1).ShouldBe(1);
        _fibService.RecursiveFib(2).ShouldBe(1);
        _fibService.RecursiveFib(3).ShouldBe(2);
        _fibService.RecursiveFib(20).ShouldBe(6765);
    }

    [Fact]
    public void RecursiveFibWithCache()
    {
        _fibService.RecursiveFibWithCache(0).ShouldBe(0);
        _fibService.RecursiveFibWithCache(1).ShouldBe(1);
        _fibService.RecursiveFibWithCache(2).ShouldBe(1);
        _fibService.RecursiveFibWithCache(3).ShouldBe(2);
        _fibService.RecursiveFibWithCache(20).ShouldBe(6765);

        _fibService.RecursiveFibWithCache(47).ShouldBe(2971215073);

        // should be using the cache
        _cache.Cache[typeof(BigInteger)].Count.ShouldBeGreaterThan(0);
    }
}
