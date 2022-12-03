using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;

namespace SmartPartyFrontend.Pages;

public class PeopleCounterModel : PageModel
{
    private readonly ILogger<PeopleCounterModel> _logger;
    private readonly HttpClient _client = new();
    
    public List<PeopleCounterRecord> Measurements { get; set; }

    public PeopleCounterModel(ILogger<PeopleCounterModel> logger)
    {
        this._logger = logger;
        Measurements = new List<PeopleCounterRecord>();
    }
    
    public void OnGet()
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/peopleCounter").Result;
        var body = response.Content.ReadFromJsonAsync<List<PeopleCounterRecord>>().Result;
        if (body != null) Measurements = body;
    }
}