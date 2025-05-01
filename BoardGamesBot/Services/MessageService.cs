using BoardGamesBot.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BoardGamesBot.Services;

public class MessageService(ITelegramBotClient botClient, ILogger<MessageService> logger)
    : IMessageService
{
    public async Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        await botClient.SendMessage(chatId, message, cancellationToken: cancellationToken);
        logger.LogInformation($"Chat {chatId}: Sent message: {message}");
    }
    
    public async Task SendMessageAsync(long chatId, string message, ParseMode parseMode, CancellationToken cancellationToken)
    {
        await botClient.SendMessage(chatId, message, ParseMode.MarkdownV2, cancellationToken: cancellationToken);
        logger.LogInformation($"Chat {chatId}: Sent message: {message}");
    }

    public async Task SendMessageAsync(string callbackQueryId, string responseMessage, CancellationToken cancellationToken)
    {
        await botClient.AnswerCallbackQuery(
            callbackQueryId: callbackQueryId,
            text: responseMessage, 
            cancellationToken: cancellationToken);
    }
    
    public async Task SendMessageAsync(long userId, string message, InlineKeyboardMarkup replyMarkup, CancellationToken cancellationToken)
    {
        await botClient.SendMessage(
            chatId: userId,
            text: message,
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken
        );
    }
    
    public async Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken)
    {
        await botClient.DeleteMessage(chatId, messageId, cancellationToken: cancellationToken);
        logger.LogInformation($"Chat {chatId}: Deleted message: {messageId}");
    }
}
