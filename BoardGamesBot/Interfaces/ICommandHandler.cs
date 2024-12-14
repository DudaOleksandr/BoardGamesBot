using Telegram.Bot;
using Telegram.Bot.Types;

namespace BoardGamesBot.Interfaces;

public interface ICommandHandler
{
    Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken);
}