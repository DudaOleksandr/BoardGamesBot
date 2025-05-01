using BoardGamesBot.Handlers.QueryHandler.Interfaces;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.QueryHandler.CallbackHandlers;

public class JoinEventCallbackHandler : ICallbackQueryHandler
{
    private readonly IEventService _eventService;
    private readonly IMessageService _messageService;

    public JoinEventCallbackHandler(
        IEventService eventService,
        IMessageService messageService)
    { 
        _eventService = eventService;
        _messageService = messageService;
    }
    
    public async Task HandleCallbackAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var data = callbackQuery.Data; // Наприклад, "join_event:1:Going"
        var parts = data.Split(':');

        if (parts.Length != 3 || parts[0].ToLower() != "joinevent" || !int.TryParse(parts[1], out var eventId))
        {
            await _messageService.SendMessageAsync(callbackQuery.From.Id, "Невірна дія.", cancellationToken);
            return;
        }

        _ = bool.TryParse(parts[2], out var isAccepted);
        await _eventService.AddParticipantAsync(eventId, callbackQuery.From.Id, isAccepted);
        
        var response = isAccepted ? "Ви приєдналися до івенту!" : "Ви відмовилися від участі.";
        await _messageService.SendMessageAsync(callbackQuery.Id, response, cancellationToken);
    }
}