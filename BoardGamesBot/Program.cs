using BoardGamesBot.Handlers.CommandHandlers;
using BoardGamesBot.Handlers.CommandHandlers.Commands;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Handlers.UserStatesHandlers;
using BoardGamesBot.Handlers.UserStatesHandlers.Interfaces;
using BoardGamesBot.Handlers.UserStatesHandlers.UserStates;
using BoardGamesBot.Infrastructure;
using BoardGamesBot.Services;
using BoardGamesBot.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(logging =>
        {
            logging.ClearProviders(); // Clear default providers
            logging.AddConsole();    // Add console logging
        });
        services.AddInfrastructureServices(context.Configuration);
        ConfigureServices(services, context.Configuration);
    })
    .Build();

await host.RunAsync();
return;


static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<ITelegramBotClient>(_ =>
    {
        var botToken = configuration["TelegramBotToken"];
        return new TelegramBotClient(botToken);
    });
    services.AddSingleton<ICommandMappingService, CommandMappingService>();
    services.AddSingleton<IUserStateService, UserStateService>();
    services.AddSingleton<IMessageService, MessageService>();
    services.AddSingleton<ICommandHandler, CreateRoomCommandHandler>();
    services.AddSingleton<ICommandHandler, JoinRoomCommandHandler>();
    services.AddSingleton<ICommandHandler, HelpCommandHandler>();
    services.AddSingleton<CommandDispatcher>();
    services.AddSingleton<IUserStateHandler, AwaitingRoomNameStateHandler>();
    services.AddSingleton<UserStateDispatcher>();
    services.AddHostedService<BotService>(); // Register BotService as a hosted service

}