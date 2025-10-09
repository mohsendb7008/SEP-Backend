using Autofac;
using FakeItEasy;
using FluentAssertions;
using SEP_Backend.HealthCheck;
using Xunit;

namespace Tests;

public class HealthCheckTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder
            .RegisterInstance(A.Fake<IHealthCheckService>())
            .As<IHealthCheckService>()
            .SingleInstance();
    });
    private readonly IHealthCheckService _healthCheckService = Container.Resolve<IHealthCheckService>(); 

    [Theory]
    [InlineData("healthy")]
    [InlineData("unhealthy")]
    [InlineData("no-data")]
    public void Get_WithFakedResponse_ReturnsResponse(string response)
    {
        A.CallTo(() => _healthCheckService.Get()).Returns(response);
        _healthCheckService.Get().Should().Be(response);
    }
}