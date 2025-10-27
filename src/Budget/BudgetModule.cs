using Autofac;

namespace SEP_Backend.Budget;

public class BudgetModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<BudgetRepository>()
            .As<IBudgetRepository>()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<BudgetService>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}