namespace SEP_Backend.Common;

public record ValidationResult(bool IsValid, string? Error)
{
    public static implicit operator ValidationResult(bool isValid) => new(isValid, null);
    public static implicit operator ValidationResult(string error) => new(false, error);
}