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
        
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<INotificationService, NotificationService>();
    }
}