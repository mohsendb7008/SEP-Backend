using Autofac;
using FakeItEasy;
using FluentAssertions;
using SEP_Backend.Common;
using SEP_Backend.Review;
using Xunit;
using TimeProvider = SEP_Backend.Common.TimeProvider;

namespace Tests.Review;

public class ReviewFactoryTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<GuidProvider>()).SingleInstance();
        builder.RegisterInstance(A.Fake<TimeProvider>()).SingleInstance();
    });
    private readonly GuidProvider _guidProvider = Container.Resolve<GuidProvider>();
    private readonly TimeProvider _timeProvider = Container.Resolve<TimeProvider>();
    private readonly ReviewFactory _reviewFactory = Container.Resolve<ReviewFactory>();

    [Fact]
    public void Create_WithArgs_ReflectsCorrectData()
    {
        var guid = Guid.Parse("69b33818-fdd9-4932-8a05-7f8ed32a2cbc");
        A.CallTo(() => _guidProvider.New()).Returns(guid);
        var time = DateTime.UnixEpoch;
        A.CallTo(() => _timeProvider.Now()).Returns(time);
        var eventId = Guid.Parse("6e5e49ef-bddb-4794-808e-8ad1c08c88d9");
        const string comments = "SomeComments";

        var review = _reviewFactory.Create(eventId, comments);

        review.Should().BeEquivalentTo(new
        {
            Id = guid,
            EventId = eventId,
            Comments = comments,
            SubmittedAt = time
        });
    }
}