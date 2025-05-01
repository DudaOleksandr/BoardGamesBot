using System.Globalization;
using BoardGamesBot.Infrastructure.Interfaces;

namespace BoardGamesBot.Infrastructure.Entities;

public class Event : IDbEntity
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public string Name { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Description { get; set; }
    
    // Navigation property
    public ICollection<EventParticipant> Participants { get; set; }
    public ICollection<Notification> Notifications { get; set; }

    public override string ToString()
    {
        return $"Event: {Name}, {Description}. Date: {ScheduledDate.ToString(CultureInfo.CurrentCulture)}";
    }
}