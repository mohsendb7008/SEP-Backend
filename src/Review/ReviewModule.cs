using Autofac;

namespace SEP_Backend.Review;

public class ReviewModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ReviewFactory>().SingleInstance();
        builder
            .RegisterType<ReviewRepository>()
            .As<IReviewRepository>()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<ReviewService>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}