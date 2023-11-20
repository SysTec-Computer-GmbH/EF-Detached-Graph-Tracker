using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

public class RootNodeWithMultipleNestedCompositionsOfSameType : IdBase, ICloneable
{
    // Properties are named with the prefix A_ and B_ because graph traversal is done alphabetically
    // This way testcases can also ensure that the order of the properties does not matter

    public ExtraLayerItem A_ExtraLayerItem { get; set; }

    public List<Item> B_CompositionItems { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithMultipleNestedCompositionsOfSameType)MemberwiseClone();
        clone.A_ExtraLayerItem = (ExtraLayerItem)A_ExtraLayerItem.Clone();
        clone.B_CompositionItems = B_CompositionItems.Select(x => (Item)x.Clone()).ToList();
        return clone;
    }
}