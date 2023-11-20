using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class AssociationRoot : IdBase, ICloneable
{
    public ExtraLayerItem Composition { get; set; }

    public object Clone()
    {
        var clone = (AssociationRoot)MemberwiseClone();
        clone.Composition = (ExtraLayerItem)Composition.Clone();
        return clone;
    }
}