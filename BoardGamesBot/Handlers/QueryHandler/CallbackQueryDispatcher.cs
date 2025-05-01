using BoardGamesBot.Handlers.QueryHandler.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.QueryHandler;

public class CallbackQueryDispatcher
{
    private readonly IMessageService _messageService;
    private readonly Dictionary<string, ICallbackQueryHandler> _queryHandlers;

    public CallbackQueryDispatcher(IEnumerable<ICallbackQueryHandler> stateHandlers, IMessageService messageService)
    {
        _messageService = messageService;
        _queryHandlers = stateHandlers.ToDictionary(handler => handler.GetType().Name.Replace("CallbackHandler", "").ToLower());
    }

    public async Task DispatchAsync(Update update, CancellationToken cancellationToken)
    {
        if (update.CallbackQuery is null) return;
        
        var data = update.CallbackQuery.Data;
        var callbackName = data?.Split(':')[0].Replace("_", string.Empty) ?? string.Empty;
        
        if (_queryHandlers.TryGetValue(callbackName, out var handler))
        {
            await handler.HandleCallbackAsync(update.CallbackQuery, cancellationToken);
            await _messageService.DeleteMessageAsync(update.Message.Chat.Id, update.Message.Id, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException($"Обробник для черги '{callbackName}' не знайдено.");
        }
    }
}