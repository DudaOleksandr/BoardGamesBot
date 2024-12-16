using BoardGamesBot.Infrastructure.Entities;

namespace BoardGamesBot.Infrastructure.Services.Interfaces;

public interface IRoomService
{
    Task<Room> CreateRoomAsync(string name, long creatorId);
    Task<Room?> GetRoomAsync(int roomId);
    Task<Room?> GetRoomByNameAsync(string roomName);
    Task AddMemberToRoomAsync(int roomId, long userId);
}
