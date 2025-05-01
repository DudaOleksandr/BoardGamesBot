using BoardGamesBot.Infrastructure.Entities;

namespace BoardGamesBot.Infrastructure.Services.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<(Event, Notification)>> GetEventRelatedNotifications(int timeToNotification);
}