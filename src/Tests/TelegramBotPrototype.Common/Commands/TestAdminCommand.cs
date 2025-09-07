using TelegramBotPrototype.SDK.Commands;

namespace TelegramBotPrototype.Common.Commands;

public class TestAdminCommand : BaseCommand
{
    public override string Path => "test_admin";
    public override bool AdminOnly => true;
    public override bool RequiresState => false;

    public override Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
        => Task.CompletedTask;
}
