using SEP_Backend.Common;
using SEP_Backend.Event;
using SEP_Backend.User;
using TimeProvider = SEP_Backend.Common.TimeProvider;

namespace SEP_Backend.ETask;

public class TaskBatchValidator(IUserRepository userRepository, IEventRepository eventRepository,
    TimeProvider timeProvider)
{
    public virtual async Task<ValidationResult> IsValid(params ETask[] tasks)
    {
        if (tasks.Length == 0)
        {
            return "No tasks found";
        }

        var eventId = tasks[0].EventId;
        foreach (var task in tasks)
        {
            if (task.UserId != Guid.Empty)
            {
                var user = await userRepository.GetByIdAsync(task.UserId);
                if (user == null)
                {
                    return "User not found";
                }
            }

            if (task.EventId != eventId)
            {
                return "All tasks must have the same event id";
            }
            var @event = await eventRepository.GetByIdAsync(task.EventId);
            if (@event == null)
            {
                return "Event not found";
            }

            if (string.IsNullOrEmpty(task.Title))
            {
                return "Task title cannot be empty";
            }

            var now = timeProvider.Now();
            if (task.DueDate < now)
            {
                return "Task due date cannot be in the past";
            }
        }

        return true;
    }
}