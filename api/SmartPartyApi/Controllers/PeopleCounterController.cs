using Microsoft.AspNetCore.Mvc;
using SmartPartyApi.Models;
using SmartPartyApi.Services;

namespace SmartPartyApi.Controllers;

[ApiController]
[Route("api/1/[controller]")]
public class PeopleCounterController : ControllerBase {

    private readonly PeopleCounterSensorService _peopleCounterSensorService;
    private readonly ILogger<PeopleCounterController> _logger;

    public PeopleCounterController(ILogger<PeopleCounterController> logger,
                                    PeopleCounterSensorService peopleCounterSensorService)
    {
        this._logger = logger;
        this._peopleCounterSensorService = peopleCounterSensorService;
    }

    [HttpGet]
    public async Task<List<PeopleCounterRecord>> Get() =>
        await _peopleCounterSensorService.GetAllPeopleCounterRecords();
}