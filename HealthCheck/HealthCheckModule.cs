using Autofac;

namespace SEP_Backend.HealthCheck;

public class HealthCheckModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<HealthCheckService>()
            .As<IHealthCheckService>()
            .SingleInstance();
    }
}