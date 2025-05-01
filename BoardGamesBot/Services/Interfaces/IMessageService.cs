using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BoardGamesBot.Services.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken);

    Task SendMessageAsync(long chatId, string message, ParseMode parseMode, CancellationToken cancellationToken);

    Task SendMessageAsync(string callbackQueryId, string responseMessage, CancellationToken cancellationToken);

    Task SendMessageAsync(long userId, string message, InlineKeyboardMarkup replyMarkup,
        CancellationToken cancellationToken);

    Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken);
}
