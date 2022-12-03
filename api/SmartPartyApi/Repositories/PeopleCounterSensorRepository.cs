namespace SmartPartyApi.Repositories;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Models;

public class PeopleCounterSensorRepository {
    private readonly IMongoCollection<PeopleCounterRecord> _peopleCounterCollection;

    public PeopleCounterSensorRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _peopleCounterCollection = mongoDb.GetCollection<PeopleCounterRecord>(databaseSettings.Value.PoepleCounterCollectionName);
    }

    public async Task<List<PeopleCounterRecord>> GetAsync() =>
        await _peopleCounterCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(PeopleCounterRecord record) =>
        await _peopleCounterCollection.InsertOneAsync(record);
}
