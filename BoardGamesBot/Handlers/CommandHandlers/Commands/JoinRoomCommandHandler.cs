using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Commands;

public class JoinRoomCommandHandler : ICommandHandler
{
    
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;
    private readonly IRoomService _roomService;

    public JoinRoomCommandHandler(IMessageService messageService, IUserStateService userStateService, IRoomService roomService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
        _roomService = roomService;
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

        var roomResult = await _roomService.GetRoomByNameAsync(roomName);

        if (roomResult is null)
        {
            await _messageService.SendMessageAsync(chatId, 
                $"Room with name: `{roomName}` was not found. Try with another name or create a new room",
                cancellationToken);
            return;
        }

        await _roomService.AddMemberToRoomAsync(roomResult.Id, chatId);
        
        await _messageService.SendMessageAsync(update.Message.Chat.Id, 
            $"Ви приєдналися до кімнати '{roomName}'.", cancellationToken: cancellationToken);
    }
}
