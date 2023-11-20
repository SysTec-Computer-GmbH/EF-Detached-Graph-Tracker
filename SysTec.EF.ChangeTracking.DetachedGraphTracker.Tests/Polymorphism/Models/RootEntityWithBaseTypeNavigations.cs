using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Models;

public class RootEntityWithBaseTypeNavigations : IdBase, ICloneable
{
    public string Text { get; set; }

    public BaseEntity? CompositionItem { get; set; }

    [ForceAggregation] public BaseEntity? AggregationItem { get; set; }

    public List<BaseEntity> CompositionItems { get; set; } = new();

    [ForceAggregation] public List<BaseEntity> AggregationItems { get; set; } = new();

    public object Clone()
    {
        var clone = (RootEntityWithBaseTypeNavigations)MemberwiseClone();
        clone.CompositionItem = (BaseEntity)CompositionItem?.Clone();
        clone.AggregationItem = (BaseEntity)AggregationItem?.Clone();
        clone.CompositionItems = CompositionItems.Select(x => (BaseEntity)x.Clone()).ToList();
        clone.AggregationItems = AggregationItems.Select(x => (BaseEntity)x.Clone()).ToList();
        return clone;
    }
}