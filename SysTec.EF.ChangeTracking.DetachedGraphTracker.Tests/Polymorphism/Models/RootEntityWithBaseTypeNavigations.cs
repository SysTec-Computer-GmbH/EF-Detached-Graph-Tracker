using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Models;

public class RootEntityWithBaseTypeNavigations : IdBase, ICloneable
{
    public string Text { get; set; }

    public BaseEntity? CompositionItem { get; set; }

    [UpdateAssociationOnly] public BaseEntity? AssociationItem { get; set; }

    public List<BaseEntity> CompositionItems { get; set; } = new();

    [UpdateAssociationOnly] public List<BaseEntity> AssociationItems { get; set; } = new();

    public object Clone()
    {
        var clone = (RootEntityWithBaseTypeNavigations)MemberwiseClone();
        clone.CompositionItem = (BaseEntity)CompositionItem?.Clone();
        clone.AssociationItem = (BaseEntity)AssociationItem?.Clone();
        clone.CompositionItems = CompositionItems.Select(x => (BaseEntity)x.Clone()).ToList();
        clone.AssociationItems = AssociationItems.Select(x => (BaseEntity)x.Clone()).ToList();
        return clone;
    }
}