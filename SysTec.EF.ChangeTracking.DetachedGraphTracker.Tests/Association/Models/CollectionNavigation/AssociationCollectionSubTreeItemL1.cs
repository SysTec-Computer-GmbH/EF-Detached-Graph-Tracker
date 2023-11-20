using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.CollectionNavigation;

public class AssociationCollectionSubTreeItemL1 : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<AssociationSubTreeItemL2> ItemsL2 { get; set; } = new();

    public object Clone()
    {
        var clone = (AssociationCollectionSubTreeItemL1)MemberwiseClone();
        clone.ItemsL2 = ItemsL2.Select(i => (AssociationSubTreeItemL2)i.Clone()).ToList();
        return clone;
    }
}