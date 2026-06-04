namespace SplitCents.Core.Models;

public class User
{
    public Guid id { get; set; }
    public string email { get; set; } = string.Empty;
    public string hashedPassword { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;

    public string GetFullName() => $"{firstName} {lastName}".Trim();

}