using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotPrototype.SDK.Commands;
using TelegramBotPrototype.SDK.Commands.Interfaces;
using TelegramBotPrototype.SDK.Options;

namespace TelegramBotPrototype.SDK;

public sealed class MessageCommandRouter
{
    private readonly UserStateManager _userStateManager;
    private readonly BotOptions _options;

    private const char _commandPrefix = '/';

    private readonly List<BaseCommand> _commandList = [];
    private readonly List<ICommandFollowUp> _commandFollowUpList = [];

    public MessageCommandRouter(UserStateManager userStateManager, IOptions<BotOptions> options)
    {
        _userStateManager = userStateManager;
        _options = options.Value;
    }

    public void RegisterCommand(BaseCommand command)
    {
        if (_commandList.Any(x => x.Path == command.Path))
            throw new ArgumentException($"Command \"{command.Path}\" is already registered", nameof(command));

        _commandList.Add(command);
    }

    public void RegisterCommandFollowUp(ICommandFollowUp followUp)
    {
        if (_commandFollowUpList.Any(x => x.FollowAfterCommand == followUp.FollowAfterCommand))
            throw new ArgumentException($"Follow-up \"{nameof(followUp)}\" is already registered");

        _commandFollowUpList.Add(followUp);
    }

    public async Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        if (message.Type != MessageType.Text)
            return;

        if (!message.Text.StartsWith(_commandPrefix))
        {
            await ExecuteFollowUpAsync(client, message, ct);
            return;
        }

        var command = _commandList.FirstOrDefault(c =>
        {
            var isAuthorized = c.AdminOnly && message.From.Id == _options.AdminId;
            var pathMatch = message.Text.TrimStart(_commandPrefix).StartsWith(c.Path, StringComparison.InvariantCultureIgnoreCase);

            return pathMatch && isAuthorized;

        }) ?? throw new KeyNotFoundException($"Couldn't find command \"{message.Text}\"");

        await command.ExecuteAsync(client, message, ct);
    }

    private async Task ExecuteFollowUpAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        var state = _userStateManager.GetUserSate(message.From.Id);
        if (state.UserState == Enums.UserState.WaitingForReply)
        {
            var previousCommandType = state.Command.GetType();

            var followUp = _commandFollowUpList.Where(x => x.FollowAfterCommand == previousCommandType).First();

            await followUp.ExecuteAsync(client, message, ct);
        }
    }
}
