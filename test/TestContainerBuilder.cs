using Autofac;
using SEP_Backend;

namespace Tests;

public static class TestContainerBuilder
{
    public static IContainer Build(Action<ContainerBuilder>? registerFakeTypes = null)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<AppModule>();
        registerFakeTypes?.Invoke(builder);
        var container = builder.Build();
        return container;
    }
}