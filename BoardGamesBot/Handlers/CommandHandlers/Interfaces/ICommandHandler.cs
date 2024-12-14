using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers.Interfaces;

public interface ICommandHandler
{
    Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken);
}