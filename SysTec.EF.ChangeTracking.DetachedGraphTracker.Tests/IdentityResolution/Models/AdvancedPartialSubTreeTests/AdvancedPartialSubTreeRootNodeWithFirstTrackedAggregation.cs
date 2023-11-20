using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;

public class AdvancedPartialSubTreeRootNodeWithFirstTrackedAggregation : IdBase, ICloneable
{
    [ForceAggregation] public AdvancedSubTreeNode1? A_Aggregation { get; set; }

    public AdvancedSubTreeNode2? B_Composition { get; set; }

    [ForceAggregation] public List<AdvancedSubTreeNode1> A_Aggregations { get; set; } = new();

    public List<AdvancedSubTreeNode2> B_Compositions { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedPartialSubTreeRootNodeWithFirstTrackedAggregation)MemberwiseClone();
        clone.A_Aggregation = (AdvancedSubTreeNode1)A_Aggregation?.Clone();
        clone.B_Composition = (AdvancedSubTreeNode2)B_Composition?.Clone();
        clone.A_Aggregations = A_Aggregations.Select(a => (AdvancedSubTreeNode1)a.Clone()).ToList();
        clone.B_Compositions = B_Compositions.Select(b => (AdvancedSubTreeNode2)b.Clone()).ToList();
        return clone;
    }
}