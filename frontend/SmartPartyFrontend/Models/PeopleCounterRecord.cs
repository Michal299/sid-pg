namespace SmartPartyFrontend.Models;

public class PeopleCounterRecord
{
    public string? Id { get; set; }
    public decimal NumberOfPeople {get; set;}
    public DateTime MeasuredAt {get; set;}
    public string? SensorId {get; set;}
}