using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppStateController : ControllerBase
{
    private readonly ILogger<AppStateController> _logger;

    public AppStateController(ILogger<AppStateController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<AppStateDto> Get()
    {
        return await Task.FromResult(new AppStateDto());
    }
}
