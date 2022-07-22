using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppStateController : ControllerBase
{
    private readonly ILogger<AppStateController> _logger;
    private readonly AppStateService _appState;
    private readonly IMapper _mapper;

    public AppStateController(
        ILogger<AppStateController> logger,
        AppStateService appState,
        IMapper mapper
    )
    {
        _logger = logger;
        _appState = appState;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<AppStateDto> Get()
    {
        var dto = _mapper.Map<AppState, AppStateDto>(_appState.State);
        return await Task.FromResult(dto);
    }
}
