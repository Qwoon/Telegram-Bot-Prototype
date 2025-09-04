using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotPrototype.SDK.Interfaces;

public interface IFollowUpCommand
{
    Type FollowAfterCommand { get; }
    Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct);
}
