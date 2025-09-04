using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using TelegramBotPrototype.Core.Data;

namespace TelegramBotPrototype.SDK.Extensions;

public static class MapperExtensions
{
    public static IQueryable<T> ProjectTo<T>(this IQueryable source, IMapper mapper, params string[] membersToExpand)
    {
        return source.ProjectTo<T>(mapper.ConfigurationProvider, null, membersToExpand);
    }

    public static void Update<T1, T2>(this IMapper mapper, T1 source, T2 destination)
    {
        var result = mapper.Map(source, destination);

        // AssignableMapper returns source so we assume not having mapping defined
        if (ReferenceEquals(source, result))
        {
            var pair = new TypePair(destination.GetType(), source?.GetType());
            throw new AutoMapperMappingException("No mapping defined for the patching!", null, pair);
        }
    }

    public static void Patch<TKey, TSource>(this IMapper mapper, TSource source, IEntity<TKey> destination)
    {
        var id = destination.Id;
        mapper.Update(source, destination);
        destination.Id = id;
    }
}
