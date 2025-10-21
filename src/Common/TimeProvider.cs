namespace SEP_Backend.Common;

public class TimeProvider
{
    public virtual DateTime Now() => DateTime.UtcNow;
}