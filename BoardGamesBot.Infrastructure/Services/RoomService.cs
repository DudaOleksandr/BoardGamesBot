using BoardGamesBot.Infrastructure.Data;
using BoardGamesBot.Infrastructure.Entities;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesBot.Infrastructure.Services;

public class RoomService(AppDbContext dbContext) : IRoomService
{
    public async Task<Room> CreateRoomAsync(string name, long creatorId)
    {
        var room = new Room
        {
            Name = name,
            CreatorId = creatorId,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Rooms.Add(room);
        await dbContext.SaveChangesAsync();
        return room;
    }

    public async Task<Room?> GetRoomAsync(int roomId)
    {
        return await dbContext.Rooms
            .Include(r => r.Members)
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    public async Task<Room?> GetRoomByNameAsync(string roomName)
    {
        return await dbContext.Rooms
            .Include(r => r.Members)
            .FirstOrDefaultAsync(r => r.Name == roomName);
    }

    public async Task AddMemberToRoomAsync(int roomId, long userId)
    {
        if (await dbContext.RoomMembers.AnyAsync(rm => rm.UserId == userId && rm.RoomId == roomId))
        {
            return;
        }
        
        var member = new RoomMember
        {
            RoomId = roomId,
            UserId = userId
        };
        
        dbContext.RoomMembers.Add(member);
        await dbContext.SaveChangesAsync();
    }
}
