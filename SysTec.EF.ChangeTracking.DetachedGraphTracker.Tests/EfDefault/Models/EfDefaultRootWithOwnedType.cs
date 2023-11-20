using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultRootWithOwnedType : IdBase, ICloneable
{
    public string Text { get; set; }

    public EfDefaultOwnedType? A_OwnedEntity { get; set; }

    public List<EfDefaultOwnedType> B_OwnedEntities { get; set; } = new();

    public object Clone()
    {
        var clone = (EfDefaultRootWithOwnedType)MemberwiseClone();
        clone.A_OwnedEntity = (EfDefaultOwnedType?)A_OwnedEntity?.Clone();
        clone.B_OwnedEntities = B_OwnedEntities.Select(ot => (EfDefaultOwnedType)ot.Clone()).ToList();
        return clone;
    }
}