using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Commands;

public class CreateRoomCommandHandler : ICommandHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;
    private readonly IRoomService _roomService;

    public CreateRoomCommandHandler(IMessageService messageService, IUserStateService userStateService, IRoomService roomService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
        _roomService = roomService;
    }
    
    public async Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        var roomName = update.Message.Text.Split(' ', 2).Skip(1).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(roomName))
        {
            _userStateService.SetState(chatId, UserState.AwaitingCreateRoomName);
            await _messageService.SendMessageAsync(update.Message.Chat.Id, "Вкажіть назву кімнати!", cancellationToken: cancellationToken);
            return;
        }

        // Логіка створення кімнати
        
        var room = await _roomService.CreateRoomAsync(roomName, chatId);
        
        await _messageService.SendMessageAsync(update.Message.Chat.Id, $"Кімната '{room.Name}' створена!", cancellationToken: cancellationToken);
    }
}
