using System.Diagnostics;
using System.Numerics;
using Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Services;

public class AppMonitorService : BackgroundService, IDisposable
{
    private Timer? _timer;
    private readonly ILogger<AppMonitorService> _logger;
    private readonly IConfiguration _config;
    private readonly AppStateService _appState;

    public AppMonitorService(
        ILogger<AppMonitorService> logger,
        IConfiguration config,
        AppStateService appState
    )
    {
        _logger = logger;
        _config = config;
        _appState = appState;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(
            o =>
            {
                _appState.UpdateAppState();
                _logger.LogInformation($"AppState: {JsonConvert.SerializeObject(_appState.State)}");
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromMilliseconds(
                _config.GetSection("AppState").GetValue<int>("MonitorPeriod_ms")
            )
        );
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        base.Dispose();
        _timer?.Dispose();
    }
}
