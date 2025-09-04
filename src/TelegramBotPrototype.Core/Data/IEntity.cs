namespace TelegramBotPrototype.Core.Data;

public interface IEntity { }
public interface IEntity<T> : IEntity
{
    /// <summary>
    /// Gets or sets entity primary key
    /// </summary>
    public T Id { get; set; }
}
