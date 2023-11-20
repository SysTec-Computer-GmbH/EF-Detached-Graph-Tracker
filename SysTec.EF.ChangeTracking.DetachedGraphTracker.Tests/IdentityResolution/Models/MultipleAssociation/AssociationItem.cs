using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleAssociation;

public class AssociationItem : IdBase, ICloneable
{
    public string Text { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}