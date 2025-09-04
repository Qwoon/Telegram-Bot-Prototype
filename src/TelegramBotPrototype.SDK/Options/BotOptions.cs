using TelegramBotPrototype.Core.Options;

namespace TelegramBotPrototype.SDK.Options;

public class BotOptions : BaseOptions<BotOptions>
{
    public long AdminGroupChatId { get; set; }
}
