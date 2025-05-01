using BoardGamesBot.Enums;
using BoardGamesBot.Handlers.UserStatesHandlers.Interfaces;
using BoardGamesBot.Infrastructure.Entities;
using BoardGamesBot.Infrastructure.Services.Interfaces;
using BoardGamesBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BoardGamesBot.Handlers.UserStatesHandlers.UserStates;

public class AwaitingEventDetailsStateHandler : IUserStateHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;
    private readonly IRoomService _roomService;
    private readonly IEventService _eventService;
    private readonly ITelegramBotClient _botClient;

    public AwaitingEventDetailsStateHandler(IMessageService messageService, IUserStateService userStateService,
        IRoomService roomService, IEventService eventService, ITelegramBotClient botClient)
    {
        _messageService = messageService;
        _userStateService = userStateService;
        _roomService = roomService;
        _eventService = eventService;
        _botClient = botClient;
    }

    public UserState State => UserState.AwaitingEventDetails;

    public async Task HandleAsync(long chatId, string message, CancellationToken cancellationToken)
    {
        // Парсимо введені дані
        var parts = message.Split('|', StringSplitOptions.TrimEntries);
        if (parts.Length != 3 || !DateTime.TryParse(parts[1], out var eventDate))
        {
            await _messageService.SendMessageAsync(chatId,
                "Невірний формат. Спробуйте ще раз:\n`Назва | Дата (YYYY-MM-DD HH:mm) | Опис`",
                cancellationToken);
            return;
        }

        var name = parts[0];
        var description = parts[2];

        // Викликаємо сервіс для створення івенту

        var userRoom = await _roomService.GetRoomByUserAsync(chatId);
        if (userRoom is null)
        {
            await _messageService.SendMessageAsync(chatId,
                $"Ви не долучені до жодної кімнати щоб створювати івенти. Спочатку долучіться або створіть нову кімнату.",
                cancellationToken);
            return;
        }
        
        var createdEvent = await _eventService.CreateEventAsync(
            roomId: userRoom.Id,
            name,
            eventDate,
            description);

        await _eventService.AddParticipantAsync(createdEvent.Id, chatId, true);
        
        // Очищуємо стан і повідомляємо про успіх
        _userStateService.ClearState(chatId);

        await _messageService.SendMessageAsync(chatId,
            $"Івент створено:\nНазва: {createdEvent.Name}\nДата: {createdEvent.ScheduledDate}\nОпис: {createdEvent.Description}",
            cancellationToken);

        var notifiedUsers = new List<string>();
        
        foreach (var member in userRoom.Members.Where(u => u.UserId != chatId))
        {
            var chatMember = await _botClient.GetChatMember(member.UserId, member.UserId, cancellationToken: cancellationToken);
            notifiedUsers.Add($"@{chatMember.User.Username}");
            await SendEventNotificationAsync(member.UserId, createdEvent);
        }
        
        var mentions = string.Join(", ", notifiedUsers);
        
        if (notifiedUsers.Count == 0)
        {
            await _messageService.SendMessageAsync(chatId,
                "Упсик, в твоїй кімнаті немає нікого крім тебе, найди друзів чортяка", 
                cancellationToken);;
            return;
        }
        
        await _messageService.SendMessageAsync(chatId,
            $"Повідомлення про створений івент були надіслані до: {mentions}",
            cancellationToken);
    }
    
    private async Task SendEventNotificationAsync(long userId, Event newEvent)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("✅ Приєднатися", $"joinEvent:{newEvent.Id}:True"),
                InlineKeyboardButton.WithCallbackData("❌ Відмовитися", $"joinEvent:{newEvent.Id}:False")
            }
        });

        var message = $"Новий івент: *{newEvent.Name}*\nДата: {newEvent.ScheduledDate}\nВи хочете приєднатися?";
        await _messageService.SendMessageAsync(userId, message, inlineKeyboard, CancellationToken.None);
    }
}