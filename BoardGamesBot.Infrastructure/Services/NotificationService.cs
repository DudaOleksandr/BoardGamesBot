using BoardGamesBot.Infrastructure.Data;
using BoardGamesBot.Infrastructure.Entities;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesBot.Infrastructure.Services;

public class NotificationService(AppDbContext dbContext) : INotificationService
{
    public async Task<IEnumerable<(Event, Notification)>> GetEventRelatedNotifications(int timeToNotification)
    {
        var eventsWithNotifications = await dbContext.Events
            .Include(e => e.Notifications)
            .ToListAsync();

        var result = eventsWithNotifications
            .SelectMany(e => e.Notifications, (e, n) => new { Event = e, Notification = n })
            .Where(x => 
                !x.Notification.IsDeleted &&
                (x.Event.ScheduledDate.AddMinutes(x.Notification.TimeDelta) - DateTime.Now).TotalMinutes <= timeToNotification)
            .Select(x => (x.Event, x.Notification));

        return result;
    }
}