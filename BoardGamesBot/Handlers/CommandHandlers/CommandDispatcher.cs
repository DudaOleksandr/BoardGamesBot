using BoardGamesBot.Interfaces;
using Telegram.Bot.Types;

namespace BoardGamesBot.Handlers.CommandHandlers;

public class CommandDispatcher
{
    private readonly IMessageService _messageService;
    private readonly Dictionary<string, ICommandHandler> _commandHandlers;
    private readonly Dictionary<string, string> _commandMappings;

    public CommandDispatcher(IEnumerable<ICommandHandler> commandHandlers, IMessageService messageService,
        ICommandMappingService commandMappingService)
    {
        _messageService = messageService;
        _commandHandlers = commandHandlers.ToDictionary(
            handler => handler.GetType().Name.Replace("CommandHandler", "").ToLower());
        _commandMappings = commandMappingService.GetCommandMappings();
    }

    public async Task DispatchAsync(long chatId, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.Text == null) return;

        var command = update.Message.Text.Split(' ')[0].TrimStart('/').ToLower();

        if (_commandHandlers.TryGetValue(command, out var handler))
        {
            await handler.HandleAsync(chatId, update, cancellationToken);
        }
        else
        {
            await _messageService.SendMessageAsync(update.Message.Chat.Id, _commandMappings["command_not_found"], cancellationToken: cancellationToken);
        }
    }
}
