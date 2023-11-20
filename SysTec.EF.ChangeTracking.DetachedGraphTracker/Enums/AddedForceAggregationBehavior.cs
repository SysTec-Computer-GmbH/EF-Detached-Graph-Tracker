using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;

/// <summary>
/// Provides configuration options for <see cref="ForceAggregationAttribute"/>.
/// </summary>
public enum AddedForceAggregationBehavior
{
    /// <summary>
    /// If the key of the entity is not set and the entity is tracked inside a <see cref="ForceAggregationAttribute"/>, <see cref="DetachedGraphTracker"/> throws an exception.
    /// </summary>
    Throw = 0,
    /// <summary>
    /// If the key of the entity is not set and the entity is tracked inside a <see cref="ForceAggregationAttribute"/>, <see cref="DetachedGraphTracker"/> does not throw an exception.<br/>
    /// The entity is kept in the <see cref="EntityState.Detached"/> state.
    /// </summary>
    Detach = 1
}