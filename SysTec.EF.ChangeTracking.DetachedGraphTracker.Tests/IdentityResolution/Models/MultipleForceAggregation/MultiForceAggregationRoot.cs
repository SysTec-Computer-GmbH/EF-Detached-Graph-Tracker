using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleForceAggregation;

public class MultiForceAggregationRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public CompositionItem CompositionItem { get; set; }

    [ForceAggregation] public ForceAggregationItem? AggregationItem { get; set; }

    [ForceAggregation] public List<ForceAggregationItem> AggregationItems { get; set; } = new();

    public object Clone()
    {
        var clone = (MultiForceAggregationRoot)MemberwiseClone();
        clone.CompositionItem = (CompositionItem)CompositionItem.Clone();
        clone.AggregationItem = (ForceAggregationItem)AggregationItem?.Clone();
        clone.AggregationItems = AggregationItems.Select(x => (ForceAggregationItem)x.Clone()).ToList();
        return clone;
    }
}