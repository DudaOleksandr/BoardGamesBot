using BoardGamesBot.Infrastructure.Interfaces;

namespace BoardGamesBot.Infrastructure.Entities;

public class EventParticipant : IDbEntity
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int RoomMemberId { get; set; }
    public RoomMember RoomMember { get; set; }
    public bool IsAccepted { get; set; }
}