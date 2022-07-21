using System.Text.Json.Serialization;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class FibController : ControllerBase
{
    private readonly ILogger<FibController> _logger;
    private readonly IFibService<int> _fibService;

    public FibController(ILogger<FibController> logger, IFibService<int> fibService)
    {
        _logger = logger;
        _fibService = fibService;
    }

    [HttpPost]
    public async Task<FibResponseDto> FibByIndex(FibRequestDto request)
    {
        var task = await Task.FromResult(
                Enumerable
                    .Range(request.StartIndex, request.EndIndex)
                    .AsParallel()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .Select(n => _fibService.RecursiveFibWithCache(n, request.Cache))
                    .ToList()
            );
        var result = new FibResponseDto()
        {
            Values = task
        };
        return result;
    }

    [HttpPost]
    /// <summary>
    /// Ignore this method, this is only to demostrate an awareness of Newtonsoft api.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<string> FibByIndexString(FibRequestDto request)
    {
        var result = new FibResponseDto()
        {
            Values = await Task.FromResult(
                Enumerable
                    .Range(request.StartIndex, request.EndIndex)
                    .AsParallel()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .Select(n => _fibService.RecursiveFibWithCache(n, request.Cache))
                    .ToList()
            )
        };
        return JsonConvert.SerializeObject(result);
    }
}
