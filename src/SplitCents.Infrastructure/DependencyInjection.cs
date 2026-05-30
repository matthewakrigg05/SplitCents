namespace SplitCents.Infrastructure;

// IServiceCollection — the DI registration list passed in from Program.cs
using Microsoft.Extensions.DependencyInjection;

// IConfiguration — read from appsettings.json / environment variables
using Microsoft.Extensions.Configuration;

// UseNpgsql — EF Core extension method for Postgres
using Microsoft.EntityFrameworkCore;

// Our DbContext — the EF Core entry point to the database
using SplitCents.Infrastructure.Data;

// Repository interface (defined in Core) and its implementation (defined here)
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Infrastructure.Repositories;

public static class DependencyInjection
{
    // Extension method on IServiceCollection so Program.cs can call builder.Services.AddInfrastructure(...)
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Register EF Core with Postgres using the connection string from appsettings.json
        services.AddDbContext<SplitCentsDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // When IUserRepository is requested, provide a UserRepository instance
        // Scoped = one instance per HTTP request
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}