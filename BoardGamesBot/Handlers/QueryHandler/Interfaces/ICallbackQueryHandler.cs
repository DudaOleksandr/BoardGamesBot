using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.QueryHandler.Interfaces;

public interface ICallbackQueryHandler
{
    Task HandleCallbackAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}
