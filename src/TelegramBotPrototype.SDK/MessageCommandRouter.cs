using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotPrototype.SDK.Commands;

namespace TelegramBotPrototype.SDK;

public sealed class MessageCommandRouter
{
    private readonly UserStateManager _userStateManager;

    private const char _commandPrefix = '/';
    private readonly List<BaseCommand> _commandList = [];

    public MessageCommandRouter(UserStateManager userStateManager)
    {
        _userStateManager = userStateManager;
    }

    public void RegisterCommand(BaseCommand command)
    {
        if (_commandList.Any(x => x.Path == command.Path))
            throw new ArgumentException($"Command \"{command.Path}\" is already registered", nameof(command));

        _commandList.Add(command);
    }

    public async Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        if (message.Type != MessageType.Text)
            return;

        if (!message.Text.StartsWith(_commandPrefix))
            return;

        var command = _commandList.FirstOrDefault(c =>
        {
            // TODO: Check if admin
            var pathMatch = message.Text.TrimStart(_commandPrefix).StartsWith(c.Path, StringComparison.InvariantCultureIgnoreCase);

            return pathMatch;

        }) ?? throw new KeyNotFoundException($"Couldn't find command \"{message.Text}\"");

        await command.ExecuteAsync(client, message, ct);
    }
}
