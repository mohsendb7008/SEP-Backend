namespace SEP_Backend.Event;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public decimal BudgetEstimate { get; set; }
    public decimal ApprovedBudget { get; set; }

    public void Update(Event @event)
    {
        Title = @event.Title;
        Description = @event.Description;
        Status = @event.Status;
        BudgetEstimate = @event.BudgetEstimate;
        ApprovedBudget = @event.ApprovedBudget;
    }
}