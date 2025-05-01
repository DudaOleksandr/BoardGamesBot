using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.CommandHandlers.Interfaces;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Commands;

public class ViewEventsCommandHandler : ICommandHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;
    private readonly IRoomService _roomService;
    private readonly IEventService _eventService;

    public ViewEventsCommandHandler(IMessageService messageService, IUserStateService userStateService,
        IRoomService roomService, IEventService eventService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
        _roomService = roomService;
        _eventService = eventService;
    }
    
    public async Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        _userStateService.SetState(chatId, UserState.None);
        
        var userRoom = await _roomService.GetRoomByUserAsync(chatId);
        if (userRoom is null)
        {
            await _messageService.SendMessageAsync(chatId,
                $"Ви не долучені до жодної кімнати щоб дивитися івенти. Спочатку долучіться або створіть нову кімнату.",
                cancellationToken);
            return;
        }
        
        var events = (await _eventService.GetEventsByRoomIdAsync(userRoom.Id)).ToList();
        
        if (!events.Any())
        {
            await _messageService.SendMessageAsync(chatId, "Немає івентів у цій кімнаті.", cancellationToken);
            return;
        }

        var response = "Івенти кімнати:\n";
        foreach (var ev in events)
        {
            response += $"- {ev.Name} ({ev.ScheduledDate}): {ev.Description}\n";
        }

        await _messageService.SendMessageAsync(chatId, response, cancellationToken);
    }
}