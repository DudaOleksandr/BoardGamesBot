using BoardGamesBot.Infrastructure.Interfaces;

namespace BoardGamesBot.Infrastructure.Entities;

public class Notification : IDbEntity
{
    public int Id { get; set; }
    
    public int EventId { get; set; }
    
    public int TimeDelta { get; set; }
    
    public bool IsDeleted { get; set; }
}