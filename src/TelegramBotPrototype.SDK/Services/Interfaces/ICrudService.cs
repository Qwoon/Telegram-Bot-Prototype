using TelegramBotPrototype.Core.Data;

namespace TelegramBotPrototype.SDK.Services.Interfaces;

public interface ICrudService<T>
    where T : class, IEntity
{
    IQueryable<T> Search();

    Task<T> InsertAsync(T entity, CancellationToken cancellation = default);
    Task<ICollection<T>> InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellation = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellation = default);
    Task<ICollection<T>> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellation = default);
    Task<T> DeleteAsync(T entity, CancellationToken cancellation = default);
    Task<ICollection<T>> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellation = default);
}

public interface ICrudService<T, TKey> : ICrudService<T>
    where T : class, IEntity<TKey>
    where TKey : struct
{
    Task<T> UpdateAsync(TKey key, Action<T> mutation, CancellationToken cancellation = default);
    Task<T> UpdateAsync<TProps>(TKey key, TProps props, Action<TProps, T> mutation, CancellationToken cancellation = default);
    Task<ICollection<T>> UpdateRangeAsync<TProps>(IDictionary<TKey, TProps> data, Action<TProps, T> mutation, CancellationToken cancellation = default);
    Task<T> DeleteAsync(TKey key, CancellationToken cancellation = default);
    Task<ICollection<T>> DeleteRangeAsync(ICollection<TKey> keys, CancellationToken cancellation = default);
}