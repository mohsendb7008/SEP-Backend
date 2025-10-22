using Autofac;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SEP_Backend.Event;
using SEP_Backend.Review;
using Xunit;

namespace Tests.Review;

public class ReviewServiceTests
{
    private static readonly IContainer Container = TestContainerBuilder.Build(builder =>
    {
        builder.RegisterInstance(A.Fake<IEventRepository>());
        builder.RegisterInstance(A.Fake<IReviewRepository>());
    });
    private readonly IEventRepository _eventRepository = Container.Resolve<IEventRepository>();
    private readonly IReviewRepository _reviewRepository = Container.Resolve<IReviewRepository>();
    private readonly ReviewService _service = Container.Resolve<ReviewService>();
    private readonly Guid _someEventId = Guid.Parse("ab1502b5-f3ab-4038-bf20-6e947495ad63");
    private const EventStatus SomeStatus = EventStatus.InProgress;
    private const string SomeComment = "SomeComment";

    [Fact]
    public async Task CreateAsync_EventDoesNotExist_ThrowsBadRequestException()
    {
        A.CallTo(() => _eventRepository.GetByIdAsync(_someEventId)).Returns((SEP_Backend.Event.Event)null);
        
        var act = () => _service.CreateAsync(_someEventId, SomeStatus, SomeComment);

        await act.Should().ThrowAsync<BadHttpRequestException>();
    }

    [Theory]
    [InlineData(EventStatus.InProgress)]
    [InlineData(EventStatus.Closed)]
    [InlineData(EventStatus.Archived)]
    [InlineData(EventStatus.Rejected)]
    public async Task CreateAsync_InvalidEventStatus_ThrowsBadRequestException(EventStatus eventStatus)
    {
        var @event = new SEP_Backend.Event.Event
        {
            Status = eventStatus 
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(_someEventId)).Returns(@event);
        
        var act = () => _service.CreateAsync(_someEventId, SomeStatus, SomeComment);
        
        await act.Should().ThrowAsync<BadHttpRequestException>();
    }

    [Theory]
    [InlineData(EventStatus.Open)]
    [InlineData(EventStatus.Closed)]
    [InlineData(EventStatus.Archived)]
    public async Task CreateAsync_InvalidDesiredStatus_ThrowsBadRequestException(EventStatus desiredStatus)
    {
        var @event = new SEP_Backend.Event.Event
        {
            Status = EventStatus.Open
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(_someEventId)).Returns(@event);
        
        var act = () => _service.CreateAsync(_someEventId, desiredStatus, SomeComment);
        
        await act.Should().ThrowAsync<BadHttpRequestException>();
    }

    [Theory]
    [InlineData(EventStatus.InProgress)]
    [InlineData(EventStatus.Rejected)]
    public async Task CreateAsync_ValidDesiredStatus_UpdatesEventAndCreatesReview(EventStatus desiredStatus)
    {
        var eventId = Guid.NewGuid();
        var @event = new SEP_Backend.Event.Event
        {
            Status = EventStatus.Open
        };
        A.CallTo(() => _eventRepository.GetByIdAsync(eventId)).Returns(@event);
        
        await _service.CreateAsync(eventId, desiredStatus, SomeComment);
        
        @event.Status.Should().Be(desiredStatus);
        A.CallTo(() => _eventRepository.UpdateAsync(@event)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _reviewRepository.CreateAsync(A<SEP_Backend.Review.Review>.That
                .Matches(review => review.EventId == eventId && review.Comments == SomeComment)))
            .MustHaveHappenedOnceExactly();
    }
}