using BoardGamesBot.Enums;
using BoardGamesBot.Interfaces;

namespace BoardGamesBot.Handlers.UserStatesHandlers;

public class UserStateDispatcher
{
    private readonly Dictionary<UserState, IUserStateHandler> _stateHandlers;

    public UserStateDispatcher(IEnumerable<IUserStateHandler> stateHandlers)
    {
        _stateHandlers = stateHandlers.ToDictionary(handler => handler.State);
    }

    public async Task DispatchAsync(long chatId, string message, UserState userState, CancellationToken cancellationToken)
    {
        if (_stateHandlers.TryGetValue(userState, out var handler))
        {
            await handler.HandleAsync(chatId, message, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException($"Обробник для стану '{userState}' не знайдено.");
        }
    }
}

