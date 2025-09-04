using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotPrototype.SDK.Interfaces;

public interface IBotUpdateHandler
{
    Task HandleAsync(Update update, CancellationToken ct);
}
