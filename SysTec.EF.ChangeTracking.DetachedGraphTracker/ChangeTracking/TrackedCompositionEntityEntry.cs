using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;

internal class TrackedCompositionEntityEntry
{
    protected TrackedCompositionEntityEntry(EntityEntryGraphNode trackedNode)
    {
        KeyValues = trackedNode.Entry.GetKeys();
        EntityEntry = trackedNode.Entry;
    }

    internal Dictionary<string, object> KeyValues { get; private set; }

    internal EntityEntry EntityEntry { get; private set; }

    internal static TrackedCompositionEntityEntry CreateCompositionOrAssociation(EntityEntryGraphNode trackedNode)
    {
        return trackedNode.IsAssociation()
            ? new TrackedAssociationEntityEntry(trackedNode)
            : new TrackedCompositionEntityEntry(trackedNode);
    }
}