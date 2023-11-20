using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;

public class AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition : IdBase, ICloneable
{
    public AdvancedSubTreeNode2? A_Composition { get; set; }

    [UpdateAssociationOnly] public AdvancedSubTreeNode1? B_Association { get; set; }

    public List<AdvancedSubTreeNode2> A_Compositions { get; set; } = new();

    [UpdateAssociationOnly] public List<AdvancedSubTreeNode1> B_Associations { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition)MemberwiseClone();
        clone.A_Composition = (AdvancedSubTreeNode2)A_Composition?.Clone();
        clone.B_Association = (AdvancedSubTreeNode1)B_Association?.Clone();
        clone.A_Compositions = A_Compositions.Select(x => (AdvancedSubTreeNode2)x.Clone()).ToList();
        clone.B_Associations = B_Associations.Select(x => (AdvancedSubTreeNode1)x.Clone()).ToList();
        return clone;
    }
}