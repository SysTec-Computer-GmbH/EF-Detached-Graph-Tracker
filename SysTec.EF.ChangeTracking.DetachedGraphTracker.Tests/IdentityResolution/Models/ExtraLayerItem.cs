using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class ExtraLayerItem : IdBase, ICloneable
{
    public TrackedItem Composition { get; set; }

    public object Clone()
    {
        var clone = (ExtraLayerItem)MemberwiseClone();
        clone.Composition = (TrackedItem)Composition.Clone();
        return clone;
    }
}