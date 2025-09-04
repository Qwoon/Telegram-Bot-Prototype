using TelegramBotPrototype.Core.Data;

namespace TelegramBotPrototype.Data.Entities;

public sealed class DailyNote : IEntity<long>
{
    public long Id { get; set; }
    public string Text { get; set; }
    public bool IsSent { get; set; }
}
