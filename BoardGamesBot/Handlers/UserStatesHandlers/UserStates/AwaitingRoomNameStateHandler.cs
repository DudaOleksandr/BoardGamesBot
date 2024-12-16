using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.UserStatesHandlers.Interfaces;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;

namespace BoardGamesBot.Handlers.UserStatesHandlers.UserStates;

public class AwaitingRoomNameStateHandler : IUserStateHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;
    private readonly IRoomService _roomService;

    public AwaitingRoomNameStateHandler(IMessageService messageService, IUserStateService userStateService, IRoomService roomService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
        _roomService = roomService;
    }

    public UserState State => UserState.AwaitingCreateRoomName;

    public async Task HandleAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        var room = await _roomService.CreateRoomAsync(message, chatId);
        await _messageService.SendMessageAsync(chatId, $"Кімната '{room.Name}' створена!", cancellationToken);
        await _roomService.AddMemberToRoomAsync(room.Id, chatId);
        
        _userStateService.ClearState(chatId);
    }
}
