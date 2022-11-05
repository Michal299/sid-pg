using Microsoft.AspNetCore.Mvc;
using SmartPartyApi.Models;
using SmartPartyApi.Services;

namespace SmartPartyApi.Controllers;

[ApiController]
[Route("api/1/[controller]")]
public class TemperatureSensorController : ControllerBase{

    private readonly TemperatureSensorService _temperatureSensorService;
    private readonly ILogger<TemperatureSensorController> _logger;

    public TemperatureSensorController(ILogger<TemperatureSensorController> logger,
                                        TemperatureSensorService temperatureSensorService)
    {
        this._temperatureSensorService = temperatureSensorService;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<List<TemperatureRecord>> Get() =>
        await _temperatureSensorService.GetAllTemperatureRecords();
}