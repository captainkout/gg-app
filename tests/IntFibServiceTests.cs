using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Services;
using Microsoft.AspNetCore.Components.Forms;
using Shouldly;
using Xunit;

namespace tests;

public class IntFibServiceTests
{
    private AppCacheService _cache;
    private IFibService<int> _fibService;

    public IntFibServiceTests()
    {
        _cache = new();
        _fibService = new IntFibService(_cache);
    }

    [Fact]
    public void RecursiveFib()
    {
        _fibService.RecursiveFib(0).ShouldBe(0);
        _fibService.RecursiveFib(1).ShouldBe(1);
        _fibService.RecursiveFib(2).ShouldBe(1);
        _fibService.RecursiveFib(3).ShouldBe(2);
        _fibService.RecursiveFib(20).ShouldBe(6765);
        _fibService.RecursiveFib(46).ShouldBe(1836311903);

        Assert
            .Throws<Exception>(() =>
            {
                _fibService.RecursiveFib(47);
            })
            .Message.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void RecursiveFibWithCache()
    {
        _fibService.RecursiveFibWithCache(0).ShouldBe(0);
        _fibService.RecursiveFibWithCache(1).ShouldBe(1);
        _fibService.RecursiveFibWithCache(2).ShouldBe(1);
        _fibService.RecursiveFibWithCache(3).ShouldBe(2);
        _fibService.RecursiveFibWithCache(20).ShouldBe(6765);
        _fibService.RecursiveFibWithCache(46).ShouldBe(1836311903);

        // should be using the cache
        _cache.Cache[typeof(int)].Count.ShouldBeGreaterThan(3);

        Assert
            .Throws<Exception>(() =>
            {
                _fibService.RecursiveFibWithCache(47);
            })
            .Message.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void IterativeFib_ExtensionMethod()
    {
        0.IterativeFib().ShouldBe(0);
        1.IterativeFib().ShouldBe(1);
        2.IterativeFib().ShouldBe(1);
        3.IterativeFib().ShouldBe(2);
        20.IterativeFib().ShouldBe(6765);
        46.IterativeFib().ShouldBe(1836311903);

        Assert
            .Throws<Exception>(() =>
            {
                47.IterativeFib();
            })
            .Message.ShouldNotBeNullOrEmpty();
    }
}
