using BoardGamesBot.Infrastructure.Entities;

namespace BoardGamesBot.Infrastructure.Services.Interfaces;

public interface IRoomMemberService
{
    Task<RoomMember?> GetRoomMemberById(int roomMemberId);
}