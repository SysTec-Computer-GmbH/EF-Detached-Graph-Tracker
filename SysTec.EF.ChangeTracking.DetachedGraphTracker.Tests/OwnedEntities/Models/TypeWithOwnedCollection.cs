using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Models;

public class TypeWithOwnedCollection : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<OwnedType> OwnedTypes { get; set; } = new();

    public object Clone()
    {
        var clone = (TypeWithOwnedCollection)MemberwiseClone();
        clone.OwnedTypes = OwnedTypes.Select(ot => (OwnedType)ot.Clone()).ToList();
        return clone;
    }
}