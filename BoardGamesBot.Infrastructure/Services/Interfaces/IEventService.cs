using BoardGamesBot.Infrastructure.Entities;

namespace BoardGamesBot.Infrastructure.Services.Interfaces;

public interface IEventService
{
    Task<Event> CreateEventAsync(int roomId, string name, DateTime scheduledDate, string description);
    Task<IEnumerable<Event>> GetEventsByRoomIdAsync(int roomId);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<IEnumerable<Event>> GetUpcomingEventsAsync(int minutesToEvent);
    Task<Event?> GetEventByIdAsync(int eventId);
    Task<bool> AddParticipantAsync(int eventId, long userId, bool isAccepted);
    Task<bool> UpdateParticipantStatusAsync(int eventId, long userId, bool isAccepted);
}
