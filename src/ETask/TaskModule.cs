using Autofac;

namespace SEP_Backend.ETask;

public class TaskModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<TaskRepository>()
            .As<ITaskRepository>()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<TaskBatchValidator>()
            .AsSelf()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<TaskService>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}