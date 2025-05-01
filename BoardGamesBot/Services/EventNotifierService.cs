using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;

namespace BoardGamesBot.Services;

public class EventNotifierService : IEventNotifierService
{
    private readonly IMessageService _messageService;
    private readonly INotificationService _notificationService;
    private readonly IRoomMemberService _roomMemberService;
    private readonly Timer _timer;
    private const int MinutesLeftToEvent = 30;

    public EventNotifierService(IMessageService messageService, INotificationService notificationService, IRoomMemberService roomMemberService)
    {
        _messageService = messageService;
        _notificationService = notificationService;
        _roomMemberService = roomMemberService;
        _timer = new Timer(CheckForUpcomingEvents, null, TimeSpan.Zero, TimeSpan.FromMinutes(MinutesLeftToEvent/2));
    }
    
    private async void CheckForUpcomingEvents(object? state)
    {
        var upcomingEvents = await _notificationService.GetEventRelatedNotifications(MinutesLeftToEvent);

        foreach (var en in upcomingEvents)
        {
            foreach (var roomMemberId in en.Item1.Participants.Select(p => p.RoomMemberId))
            {
                var roomMember = await _roomMemberService.GetRoomMemberById(roomMemberId);
                if(roomMember is null) continue;
                
                await _messageService.SendMessageAsync(roomMember.UserId, $"Event is cooooming: {en.Item1}", new CancellationToken());
            }
        }
    }
}