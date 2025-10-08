using Microsoft.AspNetCore.Mvc;

namespace SEP_Backend;

[ApiController]
public class HealthCheckController : Controller
{
    [HttpGet("health")]
    public string Get() => "healthy";
}