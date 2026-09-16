namespace SplitCents.Core.Models;

public class User
{
    public Guid id { get; set; }
    public string email { get; set; } = string.Empty;
    public string hashedPassword { get; set; } = string.Empty;
    public string displayName { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;

    public static User Create(string email, 
                              string displayName, 
                              string hashedPassword, 
                              string? firstName = null, 
                              string? lastName = null)
    {
        return new User
        {
            id = Guid.NewGuid()
            , email = email.Trim()
            , hashedPassword = hashedPassword
            , displayName = displayName.Trim()
            , firstName = firstName?.Trim() ?? string.Empty
            , lastName =  lastName?.Trim() ?? string.Empty
        };
    }

    public string GetFullName() => $"{firstName} {lastName}".Trim();

}