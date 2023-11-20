using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys.Models;

public class CompositeForeignKeyAssociationEntity : IdBase, ICloneable
{
    public string Text { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}