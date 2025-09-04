using TelegramBotPrototype.SDK.Interfaces;

namespace TelegramBotPrototype.Common.Commands;

public class Test1FollowUpCommand : IFollowUpCommand
{
    public Type FollowAfterCommand => typeof(Test1Command);

    public Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
