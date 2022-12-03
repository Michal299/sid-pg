using SmartPartyApi.Models;
using SmartPartyApi.Repositories;

namespace SmartPartyApi.Services;

public class TemperatureSensorService {

    private readonly TemperatureSensorRepository _temperatureRepository;

    public TemperatureSensorService(TemperatureSensorRepository temperatureRepository)
    {
        this._temperatureRepository = temperatureRepository;
    }

    public async Task AddTemperatureRecord(Message temperatureMessage)
    {
        var temperature = new TemperatureRecord
        {
            Value = temperatureMessage.MeasureValue,
            MeasuredAt = DateTime.Now,
            SensorId = temperatureMessage.SensorId
        };
        await _temperatureRepository.CreateAsync(temperature);
    }

    public async Task<List<TemperatureRecord>> GetAllTemperatureRecords()
    {
        return await _temperatureRepository.GetAsync();
    }
}