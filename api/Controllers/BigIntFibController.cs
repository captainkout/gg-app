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
    private readonly AppStateService _appState;

    public BigIntFibController(
        ILogger<BigIntFibController> logger,
        IFibService<BigInteger> fibService,
        AppStateService appState
    )
    {
        _logger = logger;
        _fibService = fibService;
        _appState = appState;
    }

    /// <summary>
    /// Base Fib Endpoint before any queue functionality
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("FibByIndex")]
    public async Task<ActionResult<FibResponseDto<BigInteger>>> FibByIndex(FibRequestDto request)
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(request.MaxTime);
        _appState.Queue.TryAdd(tokenSource, true);
        try
        {
            var task = await Task.Run(
                () =>
                    _fibService.ListFibWithCache(
                        request.StartIndex,
                        request.EndIndex,
                        request.Cache,
                        tokenSource.Token
                    ),
                tokenSource.Token
            );

            var result = new FibResponseDto<BigInteger>() { Completed = true, Values = task };
            return result;
        }
        catch (Exception e)
        {
            _logger.LogInformation("Failure");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            _appState.Queue.TryRemove(
                new KeyValuePair<CancellationTokenSource, bool>(tokenSource, true)
            );
        }
    }
}
