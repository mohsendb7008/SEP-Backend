using SEP_Backend.Common;
using TimeProvider = SEP_Backend.Common.TimeProvider;

namespace SEP_Backend.Review;

public class ReviewFactory(GuidProvider guidProvider, TimeProvider timeProvider)
{
    public Review Create(Guid eventId, string comments) => new()
    {
        Id = guidProvider.New(),
        EventId = eventId,
        Comments = comments,
        SubmittedAt = timeProvider.Now()
    };
}