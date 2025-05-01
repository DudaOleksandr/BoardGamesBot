using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Commands;

public class CreateEventCommandHandler : ICommandHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;
    private readonly IRoomService _roomService;

    public CreateEventCommandHandler(IMessageService messageService, IUserStateService userStateService, IRoomService roomService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
        _roomService = roomService;
    }
    
    public async Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        _userStateService.SetState(chatId, UserState.AwaitingEventDetails);
        await _messageService.SendMessageAsync(update.Message.Chat.Id,
            "Введіть дані івенту у форматі:\n`Назва | Дата (YYYY-MM-DD HH:mm) | Опис`", 
            cancellationToken: cancellationToken);
    }
}