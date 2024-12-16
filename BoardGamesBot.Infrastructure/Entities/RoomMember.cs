using BoardGamesBot.Infrastructure.Interfaces;

namespace BoardGamesBot.Infrastructure.Entities;

public class RoomMember : IDbEntity
{
    public int Id { get; set; }
    
    public int RoomId { get; set; }
    
    public Room Room { get; set; }
    
    public long UserId { get; set; }
}