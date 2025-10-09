using Autofac;
using SEP_Backend.HealthCheck;

namespace SEP_Backend;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<HealthCheckModule>();
    }
}