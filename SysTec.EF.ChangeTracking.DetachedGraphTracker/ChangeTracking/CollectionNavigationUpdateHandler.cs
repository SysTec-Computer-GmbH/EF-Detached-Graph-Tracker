using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;

internal class CollectionNavigationUpdateHandler
{
    private readonly HashSet<CollectionEntry> _collectionEntriesToBeUpdated;

    internal CollectionNavigationUpdateHandler()
    {
        _collectionEntriesToBeUpdated = new HashSet<CollectionEntry>();
    }

    internal void Cleanup()
    {
        _collectionEntriesToBeUpdated.Clear();
    }

    internal void PrepareListUpdateHandling(EntityEntryGraphNode node)
    {
        // If the current node is a UpdateAssociationOnly, no updates in the subtree should be made.
        if (!node.Entry.Collections.Any() || node.IsAssociation()) return;

        foreach (var collectionEntry in node.Entry.Collections)
        {
            // Shadow property collections are for example used by not explicitly configured back references.
            // The user cannot change the collection, so there is no need to update it.
            // Furthermore when the shadow property is a many-to-many collection, an error is thrown later in the TrackListUpdates method.
            if (collectionEntry.IsShadowProperty() || collectionEntry.HasForceUnchangedAttribute() || collectionEntry.IsBackreference(node))
            {
                continue;
            }

            _collectionEntriesToBeUpdated.Add(collectionEntry);
        }
    }

    internal async Task TrackListUpdates()
    {
        var collectionEntries = _collectionEntriesToBeUpdated.ToList();

        FixupJoinEntryStates(collectionEntries);

        foreach (var collectionEntry in collectionEntries)
        {
            var objectsToKeep = collectionEntry.GetNullSafeCurrentValue().Cast<object>().ToList();

            await LoadCollectionEntryOriginalValuesAndFixupRelationship(collectionEntry);

            var nullSafeCollectionEntryCurrentValue = collectionEntry.GetNullSafeCurrentValue();
            var objectsToRemove = collectionEntry.GetNullSafeCurrentValue()
                .Cast<object>()
                .Where(collectionItem => !objectsToKeep.Contains(collectionItem))
                .ToList();

            var collectionEntryHasForceForceDeleteAttribute = collectionEntry.HasForceDeleteAttribute();
            foreach (var objectToRemove in objectsToRemove)
            {
                nullSafeCollectionEntryCurrentValue.Remove(objectToRemove);

                if (collectionEntryHasForceForceDeleteAttribute)
                    collectionEntry.EntityEntry.Context.Remove(objectToRemove);
            }
        }
    }

    private static async Task LoadCollectionEntryOriginalValuesAndFixupRelationship(CollectionEntry collectionEntry)
    {
        var collectionEntryOriginalValues = await collectionEntry.LoadOriginalCollectionValues();

        foreach (var collectionEntryOriginalValue in collectionEntryOriginalValues)
        {
            var originalValueEntry = collectionEntry.EntityEntry.Context.Entry(collectionEntryOriginalValue);
            var trackedEntry =
                ChangeTrackingHelper.FindEntryInChangeTracker(originalValueEntry) ??
                originalValueEntry;

            // Every foreign key property that points to the current collection must be set to modified manually, for EF Core to 
            // recognize the relationship change.
            var fkProperties = trackedEntry.Metadata
                .GetForeignKeys()
                .Where(fk => fk.PrincipalEntityType == collectionEntry.Metadata.DeclaringEntityType)
                .SelectMany(fk => fk.Properties);

            foreach (var fkProperty in fkProperties) trackedEntry.Property(fkProperty).IsModified = true;

            var trackedEntity = trackedEntry.Entity;
            if (!collectionEntry.GetNullSafeCurrentValue().Contains(trackedEntity))
                collectionEntry.GetNullSafeCurrentValue().Add(trackedEntity);
        }
    }

    private static void FixupJoinEntryStates(List<CollectionEntry> collectionEntries)
    {
        if (!collectionEntries.Any()) return;
        
        var dbContext = collectionEntries.Select(e => e.EntityEntry.Context).First();
        
        var joinEntityTypes = collectionEntries
            .Where(s => s.Metadata is ISkipNavigation)
            .Select(s => ((ISkipNavigation)s.Metadata).JoinEntityType)
            .Distinct()
            .ToList();

        var joinEntries = dbContext.ChangeTracker.Entries()
            .Where(e => joinEntityTypes.Contains(e.Metadata))
            .ToList();

        foreach (var joinEntry in joinEntries)
            EntityEntryStateHelper.SetStateDependingOnKeyValue(joinEntry);
    }
}