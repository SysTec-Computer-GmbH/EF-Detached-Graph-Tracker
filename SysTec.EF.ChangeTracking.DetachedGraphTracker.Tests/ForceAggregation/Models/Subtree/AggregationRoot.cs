using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Subtree;

public class AssociationRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public Entity? Composition { get; set; }

    public object Clone()
    {
        var clone = (AssociationRoot)MemberwiseClone();
        clone.Composition = (Entity)Composition?.Clone();
        return clone;
    }
}