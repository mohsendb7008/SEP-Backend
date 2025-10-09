namespace SEP_Backend.HealthCheck;

public class HealthCheckService : IHealthCheckService
{
    public string Get() => "healthy";
}