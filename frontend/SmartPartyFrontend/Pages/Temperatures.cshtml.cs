using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;

namespace SmartPartyFrontend.Pages;

public class TemperaturesModel : PageModel
{
    private readonly HttpClient _client = new();

    private readonly ILogger<TemperaturesModel> _logger;

    public TemperaturesModel(ILogger<TemperaturesModel> logger)
    {
        _logger = logger;
        Measurements = new List<TemperatureRecordModel>();
    }

    public List<TemperatureRecordModel> Measurements { get; set; }

    public void OnGet()
    {
        var response = _client.GetAsync("http://localhost:8082/api/1/TemperatureSensor").Result;
        var body = response.Content.ReadFromJsonAsync<List<TemperatureRecordModel>>().Result;
        if (body != null) Measurements = body;
    }

    public List<ChartDataset> GetValues()
    {
        var datasets = (from i in Measurements
            group i by i.SensorId
            into t
            select new ChartDataset
            {
                Label = t.Key,
                XLabels = t.ToList().Select(m => m.MeasuredAt).ToList(),
                YValues = t.ToList().Select(m => m.Value).ToList()
            }).ToList();
        return datasets;
    }

    public class ChartDataset
    {
        public List<DateTime>? XLabels { get; set; }
        public List<decimal>? YValues { get; set; }
        public string? Label { get; set; }
    }
}