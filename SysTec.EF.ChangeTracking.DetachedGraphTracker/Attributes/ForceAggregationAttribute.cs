using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

/// <summary>
/// Overrides the EF-behavior so that all EntityEntries inside the navigation property subtree will be marked as unchanged.
/// <br/>
/// No change is persisted in the database beside the change of relationship associations.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ForceAggregationAttribute : Attribute
{
    internal AddedForceAggregationBehavior AddedForceAggregationBehavior { get; }
    
    /// <summary>
    /// Creates a new instance of <see cref="ForceAggregationAttribute"/>.
    /// </summary>
    /// <param name="addedForceAggregationBehavior">
    /// The behavior of what happens if an entity has no key set (not existing in the database) but is added in a <see cref="ForceAggregationAttribute"/> navigation.
    /// <br/>
    /// Defaults to <see cref="AddedForceAggregationBehavior.Throw"/>.
    /// </param>
    public ForceAggregationAttribute(AddedForceAggregationBehavior addedForceAggregationBehavior = AddedForceAggregationBehavior.Throw)
    {
        AddedForceAggregationBehavior = addedForceAggregationBehavior;
    }
}