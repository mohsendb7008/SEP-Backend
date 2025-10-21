namespace SEP_Backend.Event;

public class EventRequestValidator
{
    public bool IsValid(EventRequest request, out string? error)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            error = "Title is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            error = "Description is required";
            return false;
        }

        if (request.Budget < decimal.One)
        {
            error = "Budget is too small";
            return false;
        }

        error = null;
        return true;
    }
}