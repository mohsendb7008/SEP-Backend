using SEP_Backend.Event;

namespace SEP_Backend.Review;

public class ReviewService(IEventRepository eventRepository, IReviewRepository reviewRepository, ReviewFactory factory)
{
    public async Task CreateAsync(Guid eventId, EventStatus desiredStatus, string comments)
    {
        var @event = await eventRepository.GetByIdAsync(eventId);
        Validate(@event, desiredStatus);
        @event.Status = desiredStatus;
        await eventRepository.UpdateAsync(@event);
        var review = factory.Create(eventId, comments);
        await reviewRepository.CreateAsync(review);
    }

    private void Validate(Event.Event? @event, EventStatus desiredStatus)
    {
        if (@event == null)
            throw new BadHttpRequestException("Event not found");
        if (@event.Status != EventStatus.Open)
            throw new BadHttpRequestException("Event status should be open");
        if (desiredStatus != EventStatus.InProgress && desiredStatus != EventStatus.Rejected)
            throw new BadHttpRequestException("Desired status should either be in progress or rejected");
    }
}