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

    internal void PerformIdentityResolutionForUntrackedAssociations()
    {
        foreach (var associationEntry in _changeTrackingHandler.TrackedAssociationEntityEntries)
        {
            var existingEntryFromChangeTracker = ChangeTrackingHelper.FindEntryInChangeTracker(associationEntry.EntityEntry);
            if (existingEntryFromChangeTracker != null)
                ReplaceEntry(associationEntry.EntityEntry.Entity, associationEntry.InboundNavigationEntry!,
                    existingEntryFromChangeTracker.Entity);
            else if (associationEntry.EntityEntry.IsKeySet()) associationEntry.EntityEntry.SetStateUnchanged();
            else if (associationEntry.AddedAssociationEntryBehavior == AddedAssociationEntryBehavior.Detach)
                associationEntry.EntityEntry.SetStateDetached();
            else ThrowHelper.ThrowAddedAssociationEntryException(associationEntry);
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