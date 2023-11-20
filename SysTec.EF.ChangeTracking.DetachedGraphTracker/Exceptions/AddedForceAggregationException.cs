using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Exception thrown when an entity is tracked in an added state (no key set) but is annotated with the <see cref="ForceAggregationAttribute"/> (with the <see cref="AddedForceAggregationBehavior"/> set to <see cref="AddedForceAggregationBehavior.Throw"/>).
/// </summary>
public class AddedForceAggregationException : GraphHandlerException
{
    internal AddedForceAggregationException(TrackedAggregationEntityEntry aggregation) : base("The entity " +
        $"{aggregation.EntityEntry.Entity.GetType().Name} with key(s) {aggregation.EntityEntry.GetKeysDebugString()} " +
        $"and inbound navigation {aggregation.InboundNavigationEntry.Metadata.Name} " +
        $"cannot be tracked in an added state because it is annotated with the {nameof(ForceAggregationAttribute)}. " +
        $"Make sure the entity is persisted in the database before using it in a navigation property annotated with the {nameof(ForceAggregationAttribute)}.")
    {
    }
}