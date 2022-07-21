using System.Net;
using System.Text.Json.Serialization;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

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

    /// <summary>
    /// Base Fib Endpoint before any queue functionality
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("FibByIndex")]
    public async Task<ActionResult<FibResponseDto>> FibByIndex(FibRequestDto request)
    {
        try
        {
            var task = await Task.FromResult(
                Enumerable
                    .Range(request.StartIndex, request.EndIndex)
                    .AsParallel()
                    .AsOrdered()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .Select(n => _fibService.RecursiveFibWithCache(n, request.Cache))
                    .ToList()
            );
            var result = new FibResponseDto() { Completed = true, Values = task };
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
        var result = new FibResponseDto()
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
