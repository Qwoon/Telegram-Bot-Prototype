using TelegramBotPrototype.Core.Data;

namespace TelegramBotPrototype.Data.Entities;

public sealed class BotChat : IEntity<long>
{
    /// <inheritdoc cref="IEntity{T}.Id" />
    public long Id { get; set; }
    public long ChatId { get; set; }
}
