using TelegramBotPrototype.SDK.Commands;

namespace TelegramBotPrototype.Common.Commands;

public class Test1Command : BaseCommand
{
    public override string Path => "test_1";
    public override bool AdminOnly => false;
    public override bool RequiresState => true;

    public override Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
        => Task.CompletedTask;
}
