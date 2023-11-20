using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class RootNodeWithExtraLayerAfterAssociation : IdBase, ICloneable
{
    public TrackedItem A_Composition { get; set; }

    [UpdateAssociationOnly] public AssociationRoot? B_Association { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithExtraLayerAfterAssociation)MemberwiseClone();
        clone.A_Composition = (TrackedItem)A_Composition.Clone();
        clone.B_Association = (AssociationRoot)B_Association?.Clone();
        return clone;
    }
}