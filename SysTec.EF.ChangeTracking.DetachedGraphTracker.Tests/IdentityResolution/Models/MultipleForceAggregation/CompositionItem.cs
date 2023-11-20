using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleForceAggregation;

public class CompositionItem : IdBase, ICloneable
{
    public string Text { get; set; }

    [ForceAggregation] public ForceAggregationItem? AggregationItem { get; set; }

    [ForceAggregation] public List<ForceAggregationItem> AggregationItems { get; set; } = new();


    public object Clone()
    {
        var clone = (CompositionItem)MemberwiseClone();
        clone.AggregationItem = (ForceAggregationItem)AggregationItem?.Clone();
        return clone;
    }
}