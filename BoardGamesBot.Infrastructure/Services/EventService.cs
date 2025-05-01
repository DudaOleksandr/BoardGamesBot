using BoardGamesBot.Infrastructure.Data;
using BoardGamesBot.Infrastructure.Entities;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesBot.Infrastructure.Services;

public class EventService(AppDbContext dbContext) : IEventService
{
    public async Task<Event> CreateEventAsync(int roomId, string name, DateTime scheduledDate, string description)
    {
        var newEvent = new Event
        {
            RoomId = roomId,
            Name = name,
            ScheduledDate = scheduledDate,
            Description = description
        };

        dbContext.Events.Add(newEvent);
        await dbContext.SaveChangesAsync();
        return newEvent;
    }

    public async Task<IEnumerable<Event>> GetEventsByRoomIdAsync(int roomId)
    {
        return await dbContext.Events
            .Include(e => e.Participants)
            .Where(e => e.RoomId == roomId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await dbContext.Events
            .Include(e => e.Participants)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync(int minutesToEvent)
    {
        return await dbContext.Events
            .Include(e => e.Participants)
            .Where(e => e.ScheduledDate >= DateTime.Now - TimeSpan.FromMinutes(minutesToEvent))
            .ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int eventId)
    {
        return await dbContext.Events
            .Include(e => e.Participants)
            .ThenInclude(p => p.RoomMember)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<bool> AddParticipantAsync(int eventId, long userId, bool isAccepted)
    {
        var roomMember = await dbContext.RoomMembers.FirstOrDefaultAsync(rm => rm.UserId == userId);

        if (roomMember is null) return false;
        
        var existingParticipant = await dbContext.EventParticipants
            .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.RoomMemberId == roomMember.Id);

        if (existingParticipant is not null)
            return false;

        var newParticipant = new EventParticipant
        {
            EventId = eventId,
            RoomMemberId = roomMember.Id,
            IsAccepted = isAccepted
        };

        await dbContext.EventParticipants.AddAsync(newParticipant);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateParticipantStatusAsync(int eventId, long userId, bool isAccepted)
    {
        var roomMember = await dbContext.RoomMembers.FirstOrDefaultAsync(rm => rm.UserId == userId);

        if (roomMember is null) return false;
        
        var participant = await dbContext.EventParticipants
            .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.RoomMemberId == roomMember.Id);

        if (participant is null)
            return false;

        participant.IsAccepted = isAccepted;
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<EventParticipant>> GetParticipantsByEventIdAsync(int eventId)
    {
        return await dbContext.EventParticipants
            .Include(ep => ep.RoomMember)
            .Where(ep => ep.EventId == eventId)
            .ToListAsync();
    }
}