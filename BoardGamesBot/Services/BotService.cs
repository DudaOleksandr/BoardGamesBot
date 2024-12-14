using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers;
using BoardGamesBot.Handlers.UserStatesHandlers;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoardGamesBot.Services;

public class BotService : IHostedService
{
    private readonly ILogger<BotService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly CommandDispatcher  _commandDispatcher;
    private readonly UserStateDispatcher _userStateDispatcher;
    private readonly IUserStateService _userStateService;

    public BotService(ILogger<BotService> logger, CommandDispatcher commandDispatcher,
        IUserStateService userStateService, UserStateDispatcher userStateDispatcher, ITelegramBotClient botClient)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _userStateService = userStateService;
        _userStateDispatcher = userStateDispatcher;
        _botClient = botClient;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Bot service started.");
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            cancellationToken: cancellationToken
        );
        _logger.LogInformation("Listening for updates...");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Bot is stopping...");
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message?.Text is null) return;
        
        var chatId = update.Message.Chat.Id;
        var message = update.Message.Text;
        
        if (message.StartsWith('/'))
        {
            await _commandDispatcher.DispatchAsync(chatId, update, cancellationToken);
            return;
        }
        
        var userState = _userStateService.GetState(chatId);
        if (userState != UserState.None)
        {
            await _userStateDispatcher.DispatchAsync(chatId, message, userState, cancellationToken);
        }
    }
    
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}