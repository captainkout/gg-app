using System.Net;
using System.Numerics;
using System.Text.Json.Serialization;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BigIntFibController : ControllerBase
{
    private readonly ILogger<BigIntFibController> _logger;
    private readonly IFibService<BigInteger> _fibService;

    public BigIntFibController(
        ILogger<BigIntFibController> logger,
        IFibService<BigInteger> fibService
    )
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
    public async Task<ActionResult<FibResponseDto<BigInteger>>> FibByIndex(FibRequestDto request)
    {
        try
        {
            var task = await Task.FromResult(
                _fibService.ListFibWithCache(request.StartIndex, request.EndIndex, request.Cache)
            );
            var result = new FibResponseDto<BigInteger>() { Completed = true, Values = task };
            return result;
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
