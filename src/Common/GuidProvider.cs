namespace SEP_Backend.Common;

public class GuidProvider
{
    public virtual Guid New() => Guid.NewGuid();
}