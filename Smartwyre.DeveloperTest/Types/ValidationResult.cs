namespace Smartwyre.DeveloperTest.Types;

public record ValidationResult(bool IsValid, string Reason)
{
    public static ValidationResult Ok() => new(true, string.Empty);
    public static ValidationResult Fail(string reason) => new(false, reason);
}
