namespace SplitCents.API.DTOs;

using SplitCents.Core.DTOs;

public class RegisterResponse
{
    public UserResponse User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}
