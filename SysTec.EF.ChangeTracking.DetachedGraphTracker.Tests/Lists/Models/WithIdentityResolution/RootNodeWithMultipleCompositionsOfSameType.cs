using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

public class RootNodeWithMultipleCompositionsOfSameType : IdBase, ICloneable
{
    public Item CompositionItem { get; set; }

    public List<Item> CompositionItems { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithMultipleCompositionsOfSameType)MemberwiseClone();
        clone.CompositionItem = (Item)CompositionItem.Clone();
        clone.CompositionItems = CompositionItems.Select(x => (Item)x.Clone()).ToList();
        return clone;
    }
}