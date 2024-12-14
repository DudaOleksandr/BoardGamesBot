using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Commands;

public class JoinRoomCommandHandler : ICommandHandler
{
    
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;

    public JoinRoomCommandHandler(IMessageService messageService, IUserStateService userStateService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
    }
    
    public async Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        var parts = update.Message.Text.Split(' ', 2);
        if (parts.Length < 2)
        {
            _userStateService.SetState(chatId, UserState.AwaitingJoinRoomName);
            
            await _messageService.SendMessageAsync(update.Message.Chat.Id, 
                "Вкажіть назву кімнати! Приклад: /join_room НазваКімнати", cancellationToken: cancellationToken);
            return;
        }

        var roomName = parts[1];
        _userStateService.SetState(chatId, UserState.None);
        // Логіка долучення до кімнати
        await _messageService.SendMessageAsync(update.Message.Chat.Id, 
            $"Ви приєдналися до кімнати '{roomName}'.", cancellationToken: cancellationToken);
    }
}
