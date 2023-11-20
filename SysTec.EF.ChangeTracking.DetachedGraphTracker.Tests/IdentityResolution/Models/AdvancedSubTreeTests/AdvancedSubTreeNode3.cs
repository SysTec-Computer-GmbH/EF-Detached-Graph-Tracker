using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeNode3 : IdBase, ICloneable
{
    public string Text { get; set; }

    public AdvancedSubTreeNode4 CompositionNode { get; set; }

    public AdvancedSubTreeNode5 CompositionNode2 { get; set; }

    public object Clone()
    {
        var clone = (AdvancedSubTreeNode3)MemberwiseClone();
        clone.CompositionNode = (AdvancedSubTreeNode4)CompositionNode.Clone();
        clone.CompositionNode2 = (AdvancedSubTreeNode5)CompositionNode2.Clone();
        return clone;
    }
}