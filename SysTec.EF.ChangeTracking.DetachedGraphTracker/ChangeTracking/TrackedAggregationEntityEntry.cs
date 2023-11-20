using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;

internal class TrackedAggregationEntityEntry : TrackedCompositionEntityEntry
{
    internal TrackedAggregationEntityEntry(EntityEntryGraphNode trackedNode) : base(trackedNode)
    {
        InboundNavigationEntry = GetInboundNavigationEntry(trackedNode);
        AddedForceAggregationBehavior = trackedNode.InboundNavigationGetForceAggregationAttributeBehavior();
    }

    internal NavigationEntry InboundNavigationEntry { get;}

    internal AddedForceAggregationBehavior AddedForceAggregationBehavior { get; }

    private static NavigationEntry GetInboundNavigationEntry(EntityEntryGraphNode trackedNode)
    {
        // Note: InboundNavigation cannot be null here, because an aggregation always has a inbound navigation.
        ThrowHelper.ThrowIfSourceEntryIsNullAlthoughInboundNavigationIsSet(trackedNode);
        return trackedNode.SourceEntry!.Navigation(trackedNode.InboundNavigation!);
    }
}