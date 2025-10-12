using Autofac;
using SEP_Backend.User;

namespace SEP_Backend;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<AppDbContext>()
            .AsSelf()
            .InstancePerDependency();
        builder.RegisterModule<UserModule>();
    }
}