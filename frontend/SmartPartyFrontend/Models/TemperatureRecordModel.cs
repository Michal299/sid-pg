namespace SmartPartyFrontend.Models;

public class TemperatureRecordModel
{
    public string? Id { get; set; }
    public decimal Value {get; set;}
    public DateTime MeasuredAt {get; set;}
    public string? SensorId {get; set;}
}