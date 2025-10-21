using Autofac;

namespace SEP_Backend.Event;

public class EventModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EventFactory>().SingleInstance();
        builder.RegisterType<EventRequestValidator>().SingleInstance();
        builder
            .RegisterType<EventRepository>()
            .As<IEventRepository>()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<EventService>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}