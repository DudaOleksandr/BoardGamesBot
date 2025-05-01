using BoardGamesBot.Infrastructure.Data;
using BoardGamesBot.Infrastructure.Entities;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesBot.Infrastructure.Services;

public class RoomMemberService(AppDbContext dbContext) : IRoomMemberService
{
    public async Task<RoomMember?> GetRoomMemberById(int roomMemberId)
    {
        return await dbContext.RoomMembers
            .FirstOrDefaultAsync(r => r.Id == roomMemberId);
    }
}