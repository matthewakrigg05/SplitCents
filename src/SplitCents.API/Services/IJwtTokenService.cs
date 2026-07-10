namespace SplitCents.API.Services;

using SplitCents.Core.DTOs;

public interface IJwtTokenService
{
    string GenerateToken(UserResponse user);
}
