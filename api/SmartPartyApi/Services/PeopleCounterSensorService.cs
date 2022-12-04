using SmartPartyApi.Models;
using SmartPartyApi.Repositories;

namespace SmartPartyApi.Services;

public class PeopleCounterSensorService {


    private readonly PeopleCounterSensorRepository _peopleCounterSensorRepository;

    public PeopleCounterSensorService(PeopleCounterSensorRepository peopleCounterSensorRepository)
    {
        this._peopleCounterSensorRepository = peopleCounterSensorRepository;
    }

    public async Task AddPeopleCounterRecord(Message message)
    {
        var measuredObject = new PeopleCounterRecord
        {
            SensorId = message.SensorId,
            MeasuredAt =  message.MeasureTime,
            NumberOfPeople = message.MeasureValue
        };
        await _peopleCounterSensorRepository.CreateAsync(measuredObject);
    }

    public async Task<List<PeopleCounterRecord>> GetAllPeopleCounterRecords()
    {
        return await _peopleCounterSensorRepository.GetAsync();
    }
}