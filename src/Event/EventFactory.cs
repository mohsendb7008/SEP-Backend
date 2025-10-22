using SEP_Backend.Common;
using TimeProvider = SEP_Backend.Common.TimeProvider;

namespace SEP_Backend.Event;

public class EventFactory(GuidProvider guidProvider, TimeProvider timeProvider)
{
    public virtual Event Create(EventRequest request) => new()
    {
        Id = guidProvider.New(),
        Title = request.Title,
        Description = request.Description,
        Status = EventStatus.Open,
        SubmittedAt = timeProvider.Now(),
        BudgetEstimate = request.Budget,
        ApprovedBudget = decimal.Zero
    };
}