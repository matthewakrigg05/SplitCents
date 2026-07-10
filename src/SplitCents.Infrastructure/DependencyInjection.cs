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
using SplitCents.Core.Models.Security;
using SplitCents.Infrastructure.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<SplitCentsDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        // Scoped = one instance per HTTP request
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}