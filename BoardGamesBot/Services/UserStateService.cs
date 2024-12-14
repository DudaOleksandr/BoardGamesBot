using BoardGamesBot.Enums;
using BoardGamesBot.Services.Interfaces;

namespace BoardGamesBot.Services;

public class UserStateService : IUserStateService
{
    private readonly Dictionary<long, UserState> _userStates = new();

    public void SetState(long chatId, UserState state)
    {
        _userStates[chatId] = state;
    }

    public UserState GetState(long chatId)
    {
        return _userStates.GetValueOrDefault(chatId, UserState.None);
    }

    public void ClearState(long chatId)
    {
        _userStates.Remove(chatId);
    }
}