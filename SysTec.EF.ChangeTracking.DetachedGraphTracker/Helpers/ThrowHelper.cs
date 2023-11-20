using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class ThrowHelper
{
    internal static void ThrowNoPrimaryKeyException(EntityEntry entityEntry)
    {
        throw new NoPrimaryKeyException(entityEntry);
    }

    internal static void ThrowMultipleCompositionException(Type entityType)
    {
        throw new MultipleCompositionException(entityType);
    }
    
    internal static void ThrowEntityAlreadyTrackedException(EntityEntry entityEntry)
    {
        throw new EntityAlreadyTrackedException(entityEntry);
    }

    internal static void ThrowIfIsOwnedCollectionEntry(CollectionEntry collectionEntry)
    {
        var isOwned = collectionEntry.Metadata.TargetEntityType.IsOwned();
        if (isOwned) throw new OwnedCollectionNotSupportedException(collectionEntry);
    }

    internal static void ThrowIfSourceEntryIsNullAlthoughInboundNavigationIsSet(EntityEntryGraphNode node)
    {
        if (node.SourceEntry == null) 
            throw new GraphHandlerException($"Although the node {node.Entry.DebugView} has an inbound navigation, the source entry is null.");
    }

    internal static void ThrowInvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }

    internal static void ThrowAddedForceAggregationException(TrackedAggregationEntityEntry entityEntry)
    {
        throw new AddedForceAggregationException(entityEntry);
    }

    internal static void ThrowIfPropertyInfoIsNull(PropertyInfo? propertyInfo)
    {
        if (propertyInfo == null) 
            throw new GraphHandlerException("The metadata does not contain a property info.");
    }

    internal static void ThrowIfInboundNavigationIsNull(EntityEntryGraphNode node, string callerName)
    {
        if (node.InboundNavigation == null)
        {
            throw new InvalidOperationException(
                $"The inbound navigation of the node {node.Entry.Metadata.Name} is null. This can only happen if the inbound navigation is accessed on the root node. The called method {callerName} should not be called on the root node.");
        }
    }
}