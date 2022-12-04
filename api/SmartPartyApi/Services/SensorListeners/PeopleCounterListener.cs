namespace SmartPartyApi.Services.SensorListeners;

public class PeopleCounterListener : GenericSensorListener
{
    private readonly PeopleCounterSensorService _service;

    public PeopleCounterListener(ILogger<PeopleCounterListener> logger,
                                    IConfiguration config,
                                    PeopleCounterSensorService poepleCounterSensorService)
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