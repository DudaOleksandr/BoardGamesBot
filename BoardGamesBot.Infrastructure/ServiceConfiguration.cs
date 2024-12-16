using BoardGamesBot.Infrastructure.Data;
using BoardGamesBot.Infrastructure.Services;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoardGamesBot.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration contextConfiguration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(contextConfiguration.GetConnectionString("SqlConnectionString"),
                ServerVersion.AutoDetect(contextConfiguration.GetConnectionString("SqlConnectionString"))));
        
        /*services.AddDbContext<AppDbContext>(options =>
            options.UseMySql("Server=192.168.0.6;Port=5432;Database=BoardGamesBot;User=root;Password=yzbpM7tAnykhkPUwaBZtAF7N0Qg3Tr6n4qaJwdniD0nWyEml1lAMJeEUEX7ROK8t;",
                ServerVersion.AutoDetect("Server=192.168.0.6;Port=5432;Database=BoardGamesBot;User=root;Password=yzbpM7tAnykhkPUwaBZtAF7N0Qg3Tr6n4qaJwdniD0nWyEml1lAMJeEUEX7ROK8t;")));*/
        services.AddTransient<IRoomService, RoomService>();
    }
}