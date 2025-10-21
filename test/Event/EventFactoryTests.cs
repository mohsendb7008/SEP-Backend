using Autofac;
using FakeItEasy;
using FluentAssertions;
using SEP_Backend.Common;
using SEP_Backend.Event;
using Xunit;
using TimeProvider = SEP_Backend.Common.TimeProvider;

namespace Tests.Event;

public class EventFactoryTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<GuidProvider>()).SingleInstance();
        builder.RegisterInstance(A.Fake<TimeProvider>()).SingleInstance();
    });
    private readonly GuidProvider _guidProvider = Container.Resolve<GuidProvider>();
    private readonly TimeProvider _timeProvider = Container.Resolve<TimeProvider>();
    private readonly EventFactory _eventFactory = Container.Resolve<EventFactory>();

    [Fact]
    public void Create_WithRequest_ReflectsCorrectData()
    {
        var guid = Guid.Parse("6e5e49ef-bddb-4794-808e-8ad1c08c88d9");
        A.CallTo(() => _guidProvider.New()).Returns(guid);
        var time = DateTime.UnixEpoch;
        A.CallTo(() => _timeProvider.Now()).Returns(time);
        var request = new EventRequest
        {
            Title = "SomeTitle",
            Description = "SomeDescription",
            Budget = 100
        };

        var @event = _eventFactory.Create(request);

        @event.Should().BeEquivalentTo(new
        {
           Id = guid,
           request.Title,
           request.Description,
           Status = EventStatus.Open,
           SubmittedAt = time,
           BudgetEstimate = request.Budget,
           ApprovedBudget = decimal.Zero
        });
    }
}