namespace SplitCents.API.DTOs;

using SplitCents.Core.DTOs;

public class LoginResponse
{
    public UserResponse User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}
