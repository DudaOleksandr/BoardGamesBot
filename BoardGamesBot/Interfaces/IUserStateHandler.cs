using BoardGamesBot.Enums;

namespace BoardGamesBot.Interfaces;

public interface IUserStateHandler
{
    UserState State { get; }
    Task HandleAsync(long chatId, string message, CancellationToken cancellationToken);
}
