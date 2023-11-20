using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.CollectionNavigation;

public class AssociationCollectionSubTreeRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<AssociationCollectionSubTreeItemL1> ItemsL1 { get; set; } = new();

    public object Clone()
    {
        var clone = (AssociationCollectionSubTreeRoot)MemberwiseClone();
        clone.ItemsL1 = ItemsL1.Select(i => (AssociationCollectionSubTreeItemL1)i.Clone()).ToList();
        return clone;
    }
}