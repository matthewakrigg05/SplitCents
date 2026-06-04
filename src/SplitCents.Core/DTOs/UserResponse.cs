namespace SplitCents.Core.DTOs;

public class UserResponse
{
    public Guid id { get; set; }
    public string email { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
}