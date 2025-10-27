using Autofac;
using SEP_Backend.Budget;
using SEP_Backend.Common;
using SEP_Backend.ETask;
using SEP_Backend.Event;
using SEP_Backend.Review;
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
        builder.RegisterModule<CommonModule>();
        builder.RegisterModule<UserModule>();
        builder.RegisterModule<EventModule>();
        builder.RegisterModule<ReviewModule>();
        builder.RegisterModule<TaskModule>();
        builder.RegisterModule<BudgetModule>();
    }
}