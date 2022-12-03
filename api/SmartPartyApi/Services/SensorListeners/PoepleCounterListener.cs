namespace SmartPartyApi.Services.SensorListeners;

public class PoepleCounterListener : GenericSensorListener
{
    private readonly PoepleCounterSensorService _service;

    public PoepleCounterListener(ILogger<PoepleCounterListener> logger,
                                    IConfiguration config,
                                    PoepleCounterSensorService poepleCounterSensorService)
        : base(logger, config)
    {
        this._service = poepleCounterSensorService;
    }

    protected override Task HandleMessage(Message message)
    {
        return _service.AddPeopleCounterRecord(message);
    }

    protected override string GetQueueNameProperty()
    {
        return "QueueNamePeopleCounter";
    }
}