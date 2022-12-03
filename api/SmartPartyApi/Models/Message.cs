
using System;

public class Message {

    public string SensorId {get; set;}
    public DateTime MeasureTime {get; set;}
    public int MeasureValue {get; set;}

    public override string ToString()
    {
        return $"SensorId: {SensorId}; MeasureTime: {MeasureTime}; MeasureValue: {MeasureValue}";
    }
}
