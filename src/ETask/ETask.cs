namespace SEP_Backend.ETask;

public class ETask
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public User.User? User { get; set; }
    public Event.Event? Event { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }

    public void Update(ETask task)
    {
        UserId = task.UserId;
        EventId = task.EventId;
        Title = task.Title;
        Description = task.Description;
        DueDate = task.DueDate;
    }
}