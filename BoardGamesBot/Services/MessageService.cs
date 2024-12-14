using BoardGamesBot.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace BoardGamesBot.Services;

public class MessageService(ITelegramBotClient botClient, ILogger<MessageService> logger)
    : IMessageService
{
    public async Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        await botClient.SendMessage(chatId, message, cancellationToken: cancellationToken);
        logger.LogInformation($"Chat {chatId}: Sent message: {message}");
    }
}
