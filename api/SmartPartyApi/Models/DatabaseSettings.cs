namespace SmartPartyApi.Models;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TemperatureCollectionName { get; set; } = null!;

    public string PoepleCounterCollectionName {get; set;} = null!;
}