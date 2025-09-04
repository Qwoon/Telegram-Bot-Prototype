using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotPrototype.SDK.Commands;

public abstract class BaseCommand
{
    public abstract string Path { get; }
    public abstract bool AdminOnly { get; }
    public abstract bool RequiresState { get; }

    public abstract Task ExecuteAsync(ITelegramBotClient client, Message message, CancellationToken ct);

    protected virtual async Task SendMessageAsync(ITelegramBotClient client, Message message, string? text = null) =>
        await client.SendMessage(chatId: message.Chat.Id, text: text ?? "Hello world! 🌎");
}

