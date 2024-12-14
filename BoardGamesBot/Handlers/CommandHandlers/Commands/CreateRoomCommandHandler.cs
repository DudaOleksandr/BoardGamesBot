using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Commands;

public class CreateRoomCommandHandler : ICommandHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;

    public CreateRoomCommandHandler(IMessageService messageService, IUserStateService userStateService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
    }
    
    public async Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        _userStateService.SetState(chatId, UserState.AwaitingCreateRoomName);
        var roomName = update.Message.Text.Split(' ', 2).Skip(1).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(roomName))
        {
            await _messageService.SendMessageAsync(update.Message.Chat.Id, "Вкажіть назву кімнати!", cancellationToken: cancellationToken);
            return;
        }

        // Логіка створення кімнати
        await _messageService.SendMessageAsync(update.Message.Chat.Id, $"Кімната '{roomName}' створена!", cancellationToken: cancellationToken);
    }
}
