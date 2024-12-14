namespace BoardGamesBot.Interfaces;

public interface IMessageService
{
    Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken);
}
