using System.Net;
using System.Text.Json.Serialization;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IntFibController : ControllerBase
{
    private readonly ILogger<IntFibController> _logger;
    private readonly IFibService<int> _fibService;

    public IntFibController(ILogger<IntFibController> logger, IFibService<int> fibService)
    {
        _logger = logger;
        _fibService = fibService;
    }

    /// <summary>
    /// Base Fib Endpoint before any queue/processor functionality
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("FibByIndex")]
    public async Task<ActionResult<FibResponseDto<int>>> FibByIndex(FibRequestDto request)
    {
        try
        {
            var task = await Task.FromResult(
                _fibService.ListFibWithCache(request.StartIndex, request.EndIndex, request.Cache)
            );
            var result = new FibResponseDto<int>() { Completed = true, Values = task };
            return result;
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Ignore this method, this is only to demostrate an awareness of Newtonsoft api.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("FibByIndexString")]
    public async Task<string> FibByIndexString(FibRequestDto request)
    {
        var result = new FibResponseDto<int>()
        {
            Completed = true,
            Values = await Task.FromResult(
                Enumerable
                    .Range(request.StartIndex, request.EndIndex)
                    .AsParallel()
                    .AsOrdered()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .Select(n => _fibService.RecursiveFibWithCache(n, request.Cache))
                    .ToList()
            )
        };
        return JsonConvert.SerializeObject(result);
    }
}