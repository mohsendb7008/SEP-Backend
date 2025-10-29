namespace SEP_Backend.Review;

public class Review
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Event.Event? Event  { get; set; }
    public string Comments { get; set; }
    public DateTime SubmittedAt { get; set; }
}