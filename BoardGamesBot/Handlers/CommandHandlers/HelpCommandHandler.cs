using BoardGamesBot.Enums;
using BoardGamesBot.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers;

public class HelpCommandHandler : ICommandHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserStateService _userStateService;

    public HelpCommandHandler(IMessageService messageService, IUserStateService userStateService)
    {
        _messageService = messageService;
        _userStateService = userStateService;
    }
    
    public async Task HandleAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        _userStateService.SetState(chatId, UserState.None);
        var helpText = "Доступні команди:\n" +
                       "/create_room НазваКімнати - створити кімнату\n" +
                       "/join_room НазваКімнати - приєднатися до кімнати\n" +
                       "/help - список команд";
        
        await _messageService.SendMessageAsync(update.Message.Chat.Id, helpText, cancellationToken: cancellationToken);
    }
}
