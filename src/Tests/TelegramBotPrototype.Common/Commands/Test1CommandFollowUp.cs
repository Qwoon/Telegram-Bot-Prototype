using System.Threading.Tasks;
using TelegramBotPrototype.SDK.Commands.Interfaces;

namespace TelegramBotPrototype.Common.Commands;

public class Test1CommandFollowUp : ICommandFollowUp
{
    public Type FollowAfterCommand => typeof(Test1Command);

    public Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct)
        => Task.CompletedTask;
}
