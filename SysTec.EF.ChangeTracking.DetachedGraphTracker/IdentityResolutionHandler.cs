using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker;

internal class IdentityResolutionHandler
{
    private readonly ChangeTrackingHandler _changeTrackingHandler;

    internal IdentityResolutionHandler(ChangeTrackingHandler changeTrackingHandler)
    {
        _changeTrackingHandler = changeTrackingHandler;
    }

    internal void PerformIdentityResolutionForUntrackedAggregations()
    {
        foreach (var aggregation in _changeTrackingHandler.TrackedAggregationEntityEntries)
        {
            var existingEntryFromChangeTracker = ChangeTrackingHelper.FindEntryInChangeTracker(aggregation.EntityEntry);
            if (existingEntryFromChangeTracker != null)
                ReplaceEntry(aggregation.EntityEntry.Entity, aggregation.InboundNavigationEntry!,
                    existingEntryFromChangeTracker.Entity);
            else if (aggregation.EntityEntry.IsKeySet()) aggregation.EntityEntry.SetStateUnchanged();
            else if (aggregation.AddedForceAggregationBehavior == AddedForceAggregationBehavior.Detach)
                aggregation.EntityEntry.SetStateDetached();
            else ThrowHelper.ThrowAddedForceAggregationException(aggregation);
        }
    }

    private static void ReplaceEntry(object entityToReplace,
        NavigationEntry inboundNavigationOfEntryToReplace, object replacementEntity)
    {
        switch (inboundNavigationOfEntryToReplace)
        {
            case CollectionEntry collectionEntry:
            {
                // The entity to replace is inside a collection navigation property.
                var nullSafeCurrentValue = collectionEntry.GetNullSafeCurrentValue();
                var indexToReplace = nullSafeCurrentValue.IndexOf(entityToReplace);
                nullSafeCurrentValue[indexToReplace] = replacementEntity;
                break;
            }
            case ReferenceEntry referenceEntry:
                // The entity to replace is inside a reference navigation property.
                referenceEntry.CurrentValue = replacementEntity;
                break;
        }
    }
}