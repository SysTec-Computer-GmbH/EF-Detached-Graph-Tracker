using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

public class RootNodeWithCompositionAndAssociationOfSameType : IdBase, ICloneable
{
    public Item A_Composition_Item { get; set; }

    [UpdateAssociationOnly] public List<Item> B_Association_Items { get; set; } = new();

    public object Clone()
    {
        var clone = (RootNodeWithCompositionAndAssociationOfSameType)MemberwiseClone();
        clone.A_Composition_Item = (Item)A_Composition_Item.Clone();
        clone.B_Association_Items = B_Association_Items.Select(x => (Item)x.Clone()).ToList();
        return clone;
    }
}