using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartPartyApi.Models;

public class TemperatureRecord {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public decimal Value {get; set;}
    public DateTime MeasuredAt {get; set;}
}