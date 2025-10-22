using Autofac;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SEP_Backend.Event;
using Xunit;

namespace Tests.Event;

public class EventServiceTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<IEventRepository>());
        builder.RegisterInstance(A.Fake<EventFactory>());
        builder.RegisterInstance(A.Fake<EventRequestValidator>());
    });
    private readonly IEventRepository _repository = Container.Resolve<IEventRepository>();
    private readonly EventFactory _factory = Container.Resolve<EventFactory>();
    private readonly EventRequestValidator _requestValidator = Container.Resolve<EventRequestValidator>();
    private readonly EventService _service = Container.Resolve<EventService>();

    [Fact]
    public async Task CreateAsync_ValidationFails_ThrowsBadRequestException()
    {
        var request = new EventRequest();
        string? error = null;
        A.CallTo(() => _requestValidator.IsValid(request, out error)).Returns(false);

        var act = () => _service.CreateAsync(request);

        await act.Should().ThrowAsync<BadHttpRequestException>();
    }

    [Fact]
    public async Task CreateAsync_ValidationSucceeds_CreatesEvent()
    {
        var request = new EventRequest();
        string? error = null;
        A.CallTo(() => _requestValidator.IsValid(request, out error)).Returns(true);
        var @event = new SEP_Backend.Event.Event();
        A.CallTo(() => _factory.Create(request)).Returns(@event);

        await _service.CreateAsync(request);

        A.CallTo(() => _repository.CreateAsync(@event)).MustHaveHappenedOnceExactly();
    }
}