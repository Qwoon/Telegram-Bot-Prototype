using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotPrototype.SDK.Commands.Interfaces;

public interface ICommandFollowUp
{
    Type FollowAfterCommand { get; }
    Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct);
}
