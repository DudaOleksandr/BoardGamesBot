using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.UserStatesHandlers.Interfaces;
using BoardGamesBot.Services.Interfaces;

namespace BoardGamesBot.Handlers.UserStatesHandlers.UserStates;

public class AwaitingRoomNameStateHandler : IUserStateHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;

    public AwaitingRoomNameStateHandler(IMessageService messageService, IUserStateService userStateService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
    }

    public UserState State => UserState.AwaitingCreateRoomName;

    public async Task HandleAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        // Обробляємо відповідь користувача
        await _messageService.SendMessageAsync(chatId, $"Кімната '{message}' створена!", cancellationToken);
        _userStateService.ClearState(chatId);
    }
}
