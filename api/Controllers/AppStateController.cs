using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

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
