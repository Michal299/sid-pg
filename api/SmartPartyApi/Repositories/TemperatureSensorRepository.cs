namespace SmartPartyApi.Repositories;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Models;

public class TemperatureSensorRepository {
    private readonly IMongoCollection<TemperatureRecord> _temperatureCollection;

    public TemperatureSensorRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _temperatureCollection = mongoDb.GetCollection<TemperatureRecord>(databaseSettings.Value.TemperatureCollectionName);
    }

    public async Task<List<TemperatureRecord>> GetAsync() =>
        await _temperatureCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(TemperatureRecord record) =>
        await _temperatureCollection.InsertOneAsync(record);
}
