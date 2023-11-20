using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;

internal class ChangeTrackingHandler
{
    private readonly List<TrackedCompositionEntityEntry> _trackedEntities;

    internal ChangeTrackingHandler()
    {
        _trackedEntities = new List<TrackedCompositionEntityEntry>();
    }

    internal List<TrackedAssociationEntityEntry> TrackedAssociationEntityEntries =>
        _trackedEntities.OfType<TrackedAssociationEntityEntry>().ToList();

    internal void Cleanup()
    {
        _trackedEntities.Clear();
    }

    internal void AddTrackedEntity(EntityEntryGraphNode node)
    {
        _trackedEntities.Add(TrackedCompositionEntityEntry.CreateCompositionOrAssociation(node));
    }

    internal bool HasExistingCompositionInChangeTracker(EntityEntry entry)
    {
        var existingEntryFromChangeTracker = ChangeTrackingHelper.FindEntryInChangeTracker(entry);

        if (existingEntryFromChangeTracker == null) return false;
        ThrowIfEntityIsNotTrackedByGraphHandler(entry);
        
        return true;
    }


    private void ThrowIfEntityIsNotTrackedByGraphHandler(EntityEntry entry)
    {
        var trackedEntityViaGraphTraversal = _trackedEntities
            .SingleOrDefault(te =>
                EqualityHelper.KeysAreEqual(te.KeyValues, entry.GetKeys()) &&
                te.EntityEntry.Entity.GetType() == entry.Entity.GetType() &&
                te.GetType() == typeof(TrackedCompositionEntityEntry));

        if (trackedEntityViaGraphTraversal == null)
            ThrowHelper.ThrowEntityAlreadyTrackedException(entry);
    }
}