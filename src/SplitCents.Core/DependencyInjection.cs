namespace SplitCents.Core;

// IServiceCollection — the DI registration list passed in from Program.cs
using Microsoft.Extensions.DependencyInjection;

// Service interface and implementation
using SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Services;

public static class DependencyInjection
{
    // Extension method on IServiceCollection so Program.cs can call builder.Services.AddCore()
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // When IUserService is requested, provide a UserService instance
        // Scoped = one instance per HTTP request
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}