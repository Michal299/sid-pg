using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;

namespace SmartPartyFrontend.Pages;

public class PeopleCounterModel : PageModel
{
    private readonly HttpClient _client = new();
    private readonly ILogger<PeopleCounterModel> _logger;

    public PeopleCounterModel(ILogger<PeopleCounterModel> logger)
    {
        _logger = logger;
        Measurements = new List<PeopleCounterRecord>();
    }

    public List<PeopleCounterRecord> Measurements { get; set; }

    public void OnGet()
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/peopleCounter").Result;
        var body = response.Content.ReadFromJsonAsync<List<PeopleCounterRecord>>().Result;
        if (body != null) Measurements = body;
    }
}