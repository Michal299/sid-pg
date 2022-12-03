namespace SmartPartyApi.Services.SensorListeners;

using System;

public class TemperatureSensorListener : GenericSensorListener {

    private readonly TemperatureSensorService _temperatureSensorService;

    public TemperatureSensorListener(ILogger<TemperatureSensorListener> logger,
                                        IConfiguration config,
                                        TemperatureSensorService temperatureSensorService) : base(logger, config)
    {
        this._temperatureSensorService = temperatureSensorService;
    }

    protected override Task HandleMessage(Message message)
    {
        return _temperatureSensorService.AddTemperatureRecord(message);
    }

    protected override string GetQueueNameProperty()
    {
        return "QueueNameTemperature";
    }
}
