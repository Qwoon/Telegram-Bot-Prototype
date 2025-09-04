using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotPrototype.SDK.Commands;

public class StoryCommand(UserStateManager userStateManager) : BaseCommand
{
    private readonly UserStateManager _userStateManager = userStateManager;

    public override string Path => "story";
    public override bool AdminOnly => false;
    public override bool RequiresState => true;
    public override async Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        await SendMessageAsync(client, message, "Please, tell me a story, so I can forward it to the administrators.");

        _userStateManager.SetUserState(message.From.Id, new UserCommandState { Command = this, UserState = Enums.UserState.WaitingForReply });
    }
}
