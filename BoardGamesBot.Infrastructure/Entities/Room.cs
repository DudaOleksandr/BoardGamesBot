using BoardGamesBot.Infrastructure.Interfaces;

namespace BoardGamesBot.Infrastructure.Entities;

public class Room : IDbEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public long CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public ICollection<RoomMember> Members { get; set; }
}