using IHostApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace SmartPartyApi.Services.SensorListeners;

public class TemperatureSensorListener : IHostedService, IDisposable {

    private readonly ILogger _logger;
    private readonly Task _listenerTask;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TemperatureSensorListener(ILogger<TemperatureSensorListener> logger)
    {
        this._logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();
        this._listenerTask = new Task(() => ListenerTask(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
    }

    private void ListenerTask(CancellationToken token)
    {
        int i = 0;
        while (!token.IsCancellationRequested)
        {
            _logger.Log(LogLevel.Information, $"{i} loop");
            Task.Delay(30000, token).Wait(token);
            i++;
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Listener has been started.");
        this._listenerTask.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        _logger.Log(LogLevel.Information, "Listener has been stopped.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
