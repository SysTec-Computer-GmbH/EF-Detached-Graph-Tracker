using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.ReferenceNavigation;

public class ForceAggregationReferenceComplexRootNode : IdBase, ICloneable
{
    public string Text { get; set; }

    [ForceAggregation] public ForceAggregationReferenceSubTreeRoot SubTreeRoot { get; set; }

    public object Clone()
    {
        var clone = (ForceAggregationReferenceComplexRootNode)MemberwiseClone();
        clone.SubTreeRoot = (ForceAggregationReferenceSubTreeRoot)SubTreeRoot.Clone();
        return clone;
    }
}