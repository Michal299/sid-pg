using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;

namespace SmartPartyFrontend.Pages;

public class TemperaturesModel : PageModel {

    private readonly ILogger<TemperaturesModel> _logger;
    private readonly HttpClient _client = new();
    public List<TemperatureRecordModel> Measurements { get; set; }

    public TemperaturesModel(ILogger<TemperaturesModel> logger)
    {
        this._logger = logger;
        Measurements = new List<TemperatureRecordModel>();
    }

    public void OnGet()
    {
        var response = _client.GetAsync("http://api/api/1/temperatureSensor").Result;
        var body = response.Content.ReadFromJsonAsync<List<TemperatureRecordModel>>().Result;
        if (body != null) Measurements = body;
    }
}
