namespace SplitCents.API;
using SplitCents.Infrastructure;
using SplitCents.Core;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddCore();

        var app = builder.Build();

        app.Run();
    }
}