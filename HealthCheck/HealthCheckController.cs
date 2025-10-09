using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend.HealthCheck;

[ApiController]
public class HealthCheckController(IHealthCheckService healthCheckService) : Controller
{
    [HttpGet("health")]
    public string Get() => healthCheckService.Get();
}