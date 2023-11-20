using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;

internal class TrackedAssociationEntityEntry : TrackedCompositionEntityEntry
{
    internal TrackedAssociationEntityEntry(EntityEntryGraphNode trackedNode) : base(trackedNode)
    {
        InboundNavigationEntry = GetInboundNavigationEntry(trackedNode);
        AddedAssociationEntryBehavior = trackedNode.InboundNavigationGetAddedAssociationEntryBehavior();
    }

    internal NavigationEntry InboundNavigationEntry { get;}

    internal AddedAssociationEntryBehavior AddedAssociationEntryBehavior { get; }

    private static NavigationEntry GetInboundNavigationEntry(EntityEntryGraphNode trackedNode)
    {
        // Note: InboundNavigation cannot be null here, because an association node always has a inbound navigation.
        ThrowHelper.ThrowIfSourceEntryIsNullAlthoughInboundNavigationIsSet(trackedNode);
        return trackedNode.SourceEntry!.Navigation(trackedNode.InboundNavigation!);
    }
}