namespace SplitCents.Core;

// IServiceCollection — the DI registration list passed in from Program.cs
using Microsoft.Extensions.DependencyInjection;

// Service interface and implementation
using SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // Scoped = one instance per HTTP request
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}