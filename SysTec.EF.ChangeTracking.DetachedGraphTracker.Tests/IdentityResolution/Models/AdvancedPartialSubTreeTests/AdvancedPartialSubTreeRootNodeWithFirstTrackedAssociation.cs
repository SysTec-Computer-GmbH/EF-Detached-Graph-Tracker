using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;

public class AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation : IdBase, ICloneable
{
    [UpdateAssociationOnly] public AdvancedSubTreeNode1? A_Association { get; set; }

    public AdvancedSubTreeNode2? B_Composition { get; set; }

    [UpdateAssociationOnly] public List<AdvancedSubTreeNode1> A_Associations { get; set; } = new();

    public List<AdvancedSubTreeNode2> B_Compositions { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation)MemberwiseClone();
        clone.A_Association = (AdvancedSubTreeNode1)A_Association?.Clone();
        clone.B_Composition = (AdvancedSubTreeNode2)B_Composition?.Clone();
        clone.A_Associations = A_Associations.Select(a => (AdvancedSubTreeNode1)a.Clone()).ToList();
        clone.B_Compositions = B_Compositions.Select(b => (AdvancedSubTreeNode2)b.Clone()).ToList();
        return clone;
    }
}