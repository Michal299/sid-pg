using SmartPartyApi.Models;
using SmartPartyApi.Repositories;

namespace SmartPartyApi.Services;

public class TemperatureSensorService {

    private readonly TemperatureSensorRepository _temperatureRepository;

    public TemperatureSensorService(TemperatureSensorRepository temperatureRepository)
    {
        this._temperatureRepository = temperatureRepository;
    }

    public async Task AddTemperatureRecord(int temperatureValue)
    {
        var temperature = new TemperatureRecord
        {
            Value = temperatureValue,
            MeasuredAt = DateTime.Now
        };
        await _temperatureRepository.CreateAsync(temperature);
    }

    public async Task<List<TemperatureRecord>> GetAllTemperatureRecords()
    {
        return await _temperatureRepository.GetAsync();
    }
}