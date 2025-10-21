using Autofac;

namespace SEP_Backend.Common;

public class CommonModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GuidProvider>().SingleInstance();
        builder.RegisterType<TimeProvider>().SingleInstance();
    }
}