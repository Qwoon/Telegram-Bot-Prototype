using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TelegramBotPrototype.Core.Data;

namespace TelegramBotPrototype.SDK.Services;

public abstract class ResourceService<T> where T : class, IEntity
{
    protected readonly DbContext Context;

    protected DbSet<T> Set { get; }

    protected ResourceService(DbContext context)
    {
        Context = context;
        Set = context.Set<T>();
    }

    public async Task<T> InsertAsync(T entity, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        entity = Context.Add(entity).Entity;
        await Context.SaveChangesAsync(cancellation);
        return entity;
    }

    public async Task<ICollection<T>> InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellation = default)
        => await DoInsertRangeAsync(new List<T>(entities), cancellation);

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        entity = Context.Update(entity).Entity;
        await Context.SaveChangesAsync(cancellation);
        return entity;
    }

    public async Task<ICollection<T>> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellation = default)
        => await DoUpdateRangeAsync(new List<T>(entities), cancellation);

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        entity = Context.Remove(entity).Entity;
        await Context.SaveChangesAsync(cancellation);
        return entity;
    }

    public async Task<ICollection<T>> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellation = default)
        => await DoDeleteRangeAsync(new List<T>(entities), cancellation);

    protected async Task<ICollection<T>> DoInsertRangeAsync(IList<T> entities, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        for (var i = 0; i < entities.Count; i++)
            entities[i] = Set.Add(entities[i]).Entity;

        await Context.SaveChangesAsync(cancellation);
        return entities;
    }

    protected async Task<ICollection<T>> DoUpdateRangeAsync(IList<T> entities, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        for (var i = 0; i < entities.Count; i++)
            entities[i] = Context.Update(entities[i]).Entity;

        await Context.SaveChangesAsync(cancellation);
        return entities;
    }

    protected async Task<ICollection<T>> DoDeleteRangeAsync(IList<T> entities, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        for (var i = 0; i < entities.Count; i++)
            entities[i] = Set.Remove(entities[i]).Entity;

        await Context.SaveChangesAsync(cancellation);
        return entities;
    }
}

public abstract class ResourceService<T, TKey> : ResourceService<T>
    where T : class, IEntity<TKey>
    where TKey : struct
{
    private readonly Func<T, TKey> keyOf;

    protected ResourceService(DbContext context, string key = default) : base(context)
    {
        KeyName = key ?? GetPrimaryKeyName(context);

        var field = typeof(T).GetProperty(KeyName, BindingFlags.Instance | BindingFlags.Public);
        keyOf = instance => (TKey)field.GetValue(instance);
    }

    protected string KeyName { get; }

    public virtual IQueryable<T> Search() => Set;

    public IQueryable<T> SearchKey(TKey key)
        => Search().Where(e => EF.Property<TKey>(e, KeyName).Equals(key));

    public ValueTask<T> FindAsync(TKey key, CancellationToken cancellationToken = default)
        => Set.FindAsync([key], cancellationToken);

    public async Task<T> GetAsync(TKey key, CancellationToken cancellationToken = default)
        => await FindAsync(key, cancellationToken) ?? throw new Exception(typeof(T).ToString());

    public async Task<ICollection<T>> GetAsync(ICollection<TKey> keys, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        var entities = await Search()
                .Where(e => keys.Contains(EF.Property<TKey>(e, KeyName)))
                .ToDictionaryAsync(keyOf, cancellation);

        cancellation.ThrowIfCancellationRequested();
        if (entities.Count != keys.Count)
            throw new Exception(typeof(T).ToString());

        var results = new List<T>(keys.Count);
        foreach (var key in keys)
        {
            if (!entities.TryGetValue(key, out var entity))
                throw new Exception(typeof(T).ToString());

            results.Add(entity);
        }

        return results;
    }

    public async Task<T> UpdateAsync(TKey key, Action<T> mutation, CancellationToken cancellation = default)
    {
        var entity = await GetAsync(key, cancellation);
        cancellation.ThrowIfCancellationRequested();
        mutation(entity);
        return await UpdateAsync(entity, cancellation);
    }

    public async Task<T> UpdateAsync<TProps>(TKey key, TProps props, Action<TProps, T> mutation, CancellationToken cancellation = default)
    {
        var entity = await GetAsync(key, cancellation);
        cancellation.ThrowIfCancellationRequested();
        mutation(props, entity);
        return await UpdateAsync(entity, cancellation);
    }

    public async Task<ICollection<T>> UpdateRangeAsync<TProps>(IDictionary<TKey, TProps> data, Action<TProps, T> mutation, CancellationToken cancellation = default)
    {
        var results = await GetAsync(data.Keys, cancellation);
        cancellation.ThrowIfCancellationRequested();

        foreach (var entity in results)
            mutation(data[keyOf(entity)], entity);

        return await DoUpdateRangeAsync((IList<T>)results, cancellation);
    }

    public async Task<T> DeleteAsync(TKey key, CancellationToken cancellation = default)
    {
        var entity = await GetAsync(key, cancellation);
        cancellation.ThrowIfCancellationRequested();
        return await DeleteAsync(entity, cancellation);
    }

    /// <summary> Try to delete an entity by the primary key if such exists. </summary>
    /// <param name="key">A primary key to be found.</param>
    /// <param name="cancellation">A token to observe while waiting for the task to complete.</param>
    /// <returns>A deleted entity on success; null otherwise.</returns>
    public async Task<T> TryDeleteAsync(TKey key, CancellationToken cancellation = default)
    {
        var entity = await FindAsync(key, cancellation);
        return entity != null
            ? await DeleteAsync(entity, cancellation)
            : null;
    }

    /// <summary>  Delete multiple entities from a database in a batch. </summary>
    /// <param name="keys">A collection of primary keys to delete.</param>
    /// <param name="cancellation">A token to observe while waiting for the task to complete.</param>
    /// <returns>A collection of deleted entities.</returns>
    /// <exception cref="EntityNotFoundException">When any of the entities not found by primary key.</exception>
    public async Task<ICollection<T>> DeleteRangeAsync(ICollection<TKey> keys, CancellationToken cancellation = default)
    {
        var entities = await GetAsync(keys, cancellation);
        return await DoDeleteRangeAsync((IList<T>)entities, cancellation);
    }

    private static string GetPrimaryKeyName(DbContext context)
    {
        return context.Model
            .FindEntityType(typeof(T))!
            .FindPrimaryKey()!
            .Properties
            .Single()
            .Name;
    }
};
