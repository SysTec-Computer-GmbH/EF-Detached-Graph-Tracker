using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class EntityEntryHelper
{
    internal static Dictionary<string, object> GetKeys(this EntityEntry entry)
    {
        var primaryKey = entry.Metadata.FindPrimaryKey();

        // This error should never occur, because TrackGraph throws an invalid operation exception if the entity has no primary key.
        if (primaryKey == null) ThrowHelper.ThrowNoPrimaryKeyException(entry);

        var keyProperties = entry.Properties
            .Where(p => primaryKey!.Properties.Contains(p.Metadata))
            .ToList();

        if (!keyProperties.Any() || keyProperties.Any(p => p.CurrentValue == null))
            ThrowHelper.ThrowNoPrimaryKeyException(entry);
        
        return keyProperties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue!);
    }

    internal static string GetKeysDebugString(this EntityEntry entry)
    {
        return string.Join(", ", entry.GetKeys().Select(k => $"[{k.Key}: {k.Value}]"));
    }

    /// <summary>
    ///     Implementation of IsKeySet property with support for composite keys.
    ///     If more than one key property exists, the entity is considered to have a composite key.
    ///     In this case, the database is queried to check if the entity already exists.
    ///     If the database query returns data, the entity is considered to have a key set. Otherwise not.
    ///     If no composite Key exists, the default implementation of IsKeySet is used.
    /// </summary>
    internal static bool IsKeySet(this EntityEntry entry)
    {
        var hasCompositeKey = entry.Metadata.FindPrimaryKey()?.Properties.Count > 1;

        var hasNullablePrimaryKey = entry.Metadata.FindPrimaryKey()?.Properties
            .Any(p => IsTypeNullable(p.PropertyInfo?.PropertyType)) ?? false;

        if (!hasCompositeKey && !hasNullablePrimaryKey) return entry.IsKeySet;

        var existsInDb = entry.GetDatabaseValues() != null;
        return existsInDb;
    }

    internal static bool IsAssociation(this EntityEntryGraphNode node)
    {
        return node.InboundNavigation != null && node.InboundNavigationHasUpdateAssociationOnlyAttribute();
    }

    private static bool IsTypeNullable(Type? type)
    {
        if (type == null) return true;
        return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}