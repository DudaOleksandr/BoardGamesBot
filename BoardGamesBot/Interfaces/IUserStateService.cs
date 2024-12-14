using BoardGamesBot.Enums;

namespace BoardGamesBot.Interfaces;

public interface IUserStateService
{
    void SetState(long chatId, UserState state);
    UserState GetState(long chatId);
    void ClearState(long chatId);
}
