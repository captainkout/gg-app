using System.Net;
using System.Text.Json.Serialization;
using System.Threading;
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

    private readonly AppStateService _appState;

    public IntFibController(
        ILogger<IntFibController> logger,
        IFibService<int> fibService,
        AppStateService appState
    )
    {
        _logger = logger;
        _fibService = fibService;
        _appState = appState;
    }

    /// <summary>
    /// Base Fib Endpoint before any queue/processor functionality
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("FibByIndex")]
    public async Task<ActionResult<FibResponseDto<int>>> FibByIndex(FibRequestDto request)
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
            var result = new FibResponseDto<int>() { Completed = true, Values = task };
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
