using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

public class SelfReferencingItem : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<SelfReferencingItem> Children { get; set; } = new();

    public object Clone()
    {
        var clone = (SelfReferencingItem)MemberwiseClone();
        clone.Children = Children.Select(c => (SelfReferencingItem)c.Clone()).ToList();
        return clone;
    }
}