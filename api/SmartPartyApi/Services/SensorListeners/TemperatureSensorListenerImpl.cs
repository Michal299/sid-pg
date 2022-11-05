using RabbitMQ.Client.Exceptions;

namespace SmartPartyApi.Services.SensorListeners;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

public class TemperatureSensorListener : IHostedService, IDisposable {

    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly Task _listenerTask;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TemperatureSensorService _temperatureSensorService;

    public TemperatureSensorListener(ILogger<TemperatureSensorListener> logger,
                                        IConfiguration config,
                                        TemperatureSensorService temperatureSensorService)
    {
        this._logger = logger;
        this._config = config;
        this._temperatureSensorService = temperatureSensorService;
        this._cancellationTokenSource = new CancellationTokenSource();
        this._listenerTask = new Task(() => ListenerTask(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
    }

    private void ListenerTask(CancellationToken token)
    {

        (IConnection connection, IModel channel) = ConnectToBroker(10, 2, token);

        channel.QueueDeclare(queue: "temperature_sensor",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var value = BitConverter.ToInt32(body);
            _temperatureSensorService.AddTemperatureRecord(value).Wait(token);
            _logger.Log(LogLevel.Debug, $"Received: {value}");
        };
        channel.BasicConsume(
                queue: _config.GetRequiredSection("Rabbit").GetValue<String>("QueueNameTemperature"),
                autoAck: true,
                consumer: consumer
            );

        _logger.Log(LogLevel.Information, "Start listening for temperature messages...");
        while (!token.IsCancellationRequested)
        {
            Task.Delay(1000, token).Wait(token);
        }
        channel.Close();
        connection.Close();
    }

    private (IConnection, IModel) ConnectToBroker(int numberOfRetries, int waitSecondsBetweenRetries, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config.GetRequiredSection("Rabbit").GetValue<String>("Host"),
            UserName = _config.GetRequiredSection("Rabbit").GetValue<String>("Username"),
            Password = _config.GetRequiredSection("Rabbit").GetValue<String>("Password"),
            Port =  _config.GetRequiredSection("Rabbit").GetValue<Int32>("Port")
        };

        var retries = 0;
        while (!cancellationToken.IsCancellationRequested && retries <= numberOfRetries)
        {
            try
            {
                IConnection connection = factory.CreateConnection();
                IModel channel = connection.CreateModel();
                _logger.Log(LogLevel.Information, "Succesfully obtained connection to message broker");
                return (connection, channel);
            } catch(BrokerUnreachableException exception)
            {
                _logger.Log(LogLevel.Warning, "Listener cannot connect to broker due to exception: {}. Next attempt will be made in: {}s. Already made retries: {}",
                        exception.Message,
                        waitSecondsBetweenRetries,
                        retries);
                Task.Delay((int)TimeSpan.FromSeconds(waitSecondsBetweenRetries).TotalMilliseconds, cancellationToken).Wait(cancellationToken);
                retries++;
            }
        }
        throw new SystemException("Cannot connect to message broker.");
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
        _cancellationTokenSource.Cancel();
        _logger.Log(LogLevel.Information, "Listener has been disposed.");
    }

}
