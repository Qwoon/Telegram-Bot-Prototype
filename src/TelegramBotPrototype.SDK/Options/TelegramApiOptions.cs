using TelegramBotPrototype.Core.Options;

namespace TelegramBotPrototype.SDK.Options;

public class TelegramApiOptions : BaseOptions<TelegramApiOptions>
{
    public string ApiToken { get; set; }
    public string ApiSecret { get; set; }
    public string WebhookUrl { get; set; }
}
