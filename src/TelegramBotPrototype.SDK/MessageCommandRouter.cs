using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotPrototype.SDK.Commands;
using TelegramBotPrototype.SDK.Commands.Interfaces;

namespace TelegramBotPrototype.SDK;

public sealed class MessageCommandRouter
{
    private readonly UserStateManager _userStateManager;

    private const char _commandPrefix = '/';

    private readonly List<BaseCommand> _commandList = [];
    private readonly List<ICommandFollowUp> _commandFollowUpList = [];

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
            // In case the command does not start with a prefix, then is's a follow-up command

            var state = _userStateManager.GetUserSate(message.From.Id);
            if (state.UserState == Enums.UserState.WaitingForReply)
            {
                var previousCommandType = state.Command.GetType();

                var followUp = _commandFollowUpList.Where(x => x.FollowAfterCommand == previousCommandType).First();

                await followUp.ExecuteAsync(client, message, ct);
            }

            return;
        }

        var command = _commandList.FirstOrDefault(c =>
        {
            // TODO: Check if admin
            var pathMatch = message.Text.TrimStart(_commandPrefix).StartsWith(c.Path, StringComparison.InvariantCultureIgnoreCase);

            return pathMatch;

        }) ?? throw new KeyNotFoundException($"Couldn't find command \"{message.Text}\"");

        await command.ExecuteAsync(client, message, ct);
    }
}
