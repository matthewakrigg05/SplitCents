namespace SplitCents.API;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SplitCents.Infrastructure.Data;
using SplitCents.Infrastructure;
using SplitCents.Core;
using SplitCents.API.Middleware;
using SplitCents.API.Services;
using DotNetEnv;

public class Program
{
    public static void Main(string[] args)
    {

        var envPath = Path.GetFullPath(
        Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env"));
        Env.Load(envPath);

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddCore();

        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

        builder.Services.AddControllers();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider
                .GetRequiredService<SplitCentsDbContext>();

            db.Database.Migrate();
        }

        // ExceptionMiddleware must be first so it catches exceptions from all subsequent middleware.
        app.UseMiddleware<ExceptionMiddleware>();

        // UseAuthentication must come before UseAuthorization.
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
