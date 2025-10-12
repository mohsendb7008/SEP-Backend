using Autofac;

namespace SEP_Backend.User;

public class UserModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<UserRepository>()
            .As<IUserRepository>()
            .InstancePerLifetimeScope();
    }
}