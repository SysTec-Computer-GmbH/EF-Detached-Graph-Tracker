using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeRootNodeWithFirstTrackedAggregation : IdBase, ICloneable
{
    public string Text { get; set; }

    [ForceAggregation] public AdvancedSubTreeNode1? A_AggregationNode { get; set; }

    [ForceAggregation] public List<AdvancedSubTreeNode1> A_AggregationNodes { get; set; } = new();

    public AdvancedSubTreeNode1? B_CompositionNode { get; set; }

    public object Clone()
    {
        var clone = (AdvancedSubTreeRootNodeWithFirstTrackedAggregation)MemberwiseClone();
        clone.A_AggregationNode = (AdvancedSubTreeNode1?)A_AggregationNode?.Clone();
        clone.B_CompositionNode = (AdvancedSubTreeNode1?)B_CompositionNode?.Clone();
        clone.A_AggregationNodes = A_AggregationNodes.Select(x => (AdvancedSubTreeNode1)x.Clone()).ToList();
        return clone;
    }
}