using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartPartyApi.Models;

public class PeopleCounterRecord {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public decimal NumberOfPeople {get; set;}
    public DateTime MeasuredAt {get; set;}
    [BsonRepresentation(BsonType.String)]
    public string? SensorId {get; set;}
}