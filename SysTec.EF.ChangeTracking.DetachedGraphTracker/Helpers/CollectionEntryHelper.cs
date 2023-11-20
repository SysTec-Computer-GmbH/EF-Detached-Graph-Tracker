using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class CollectionEntryHelper
{
    internal static IList GetNullSafeCurrentValue(this CollectionEntry collection)
    {
        return (IList)(collection.CurrentValue ?? new List<object>());
    }

    internal static bool IsBackreference(this CollectionEntry collection, EntityEntryGraphNode node)
    {
        return node.SourceEntry != null && node.SourceEntry.Navigations
            .Where(n => n.Metadata == collection.Metadata.Inverse)
            .Any(n => NavigationContainsEntity(n, node.Entry.Entity));
    }

    /// <summary>
    ///     The collection.load() method cannot be used, because when a collection item is present in the change tracker,
    ///     but deleted from the collection, the collection.load() method will not load the item again.
    ///     Therefore the original values are loaded and the relationship must be fixed up manually with the returned values.
    /// </summary>
    /// <param name="collectionEntry"></param>
    /// <returns></returns>
    /// <exception cref="OwnedCollectionNotSupportedException"></exception>
    internal static async Task<List<object>> LoadOriginalCollectionValues(this CollectionEntry collectionEntry)
    {
        ThrowHelper.ThrowIfIsOwnedCollectionEntry(collectionEntry);

        var query = collectionEntry.Query().Cast<object>();
        // AsSingleQuery must be used, because otherwise a NullReferenceException is thrown when the collection is part of a M:N relationship
        // https://github.com/dotnet/efcore/issues/32225
        return await query.AsSingleQuery().IgnoreAutoIncludes().ToListAsync();
    }
    
    private static bool NavigationContainsEntity(NavigationEntry navigationEntry, object entity)
    {
        if (!navigationEntry.Metadata.IsCollection) return navigationEntry.CurrentValue == entity;
        
        var nullSafeValue = (navigationEntry as CollectionEntry)!.GetNullSafeCurrentValue();
        return nullSafeValue.Cast<object>().Contains(entity);

    }
}