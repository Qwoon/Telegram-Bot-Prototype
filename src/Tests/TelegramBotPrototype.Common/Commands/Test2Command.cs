using TelegramBotPrototype.SDK.Commands;

namespace TelegramBotPrototype.Common.Commands;

public class Test2Command : BaseCommand
{
    public override string Path => "test_2";
    public override bool AdminOnly => false;
    public override bool RequiresState => true;

    public override Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
        => Task.CompletedTask;
}
