using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeNode2 : IdBase, ICloneable
{
    public string Text { get; set; }

    public AdvancedSubTreeNode3? CompositionNode { get; set; }

    public List<AdvancedSubTreeListItem1> CompositionListItems { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedSubTreeNode2)MemberwiseClone();
        clone.CompositionNode = (AdvancedSubTreeNode3)CompositionNode?.Clone();
        clone.CompositionListItems = CompositionListItems.Select(x => (AdvancedSubTreeListItem1)x.Clone()).ToList();
        return clone;
    }
}