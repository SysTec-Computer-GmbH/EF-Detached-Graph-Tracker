using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Subtree;

public class RootNode : IdBase, ICloneable
{
    [UpdateAssociationOnly] public AssociationRoot? Association { get; set; }

    [UpdateAssociationOnly] public List<AssociationRoot> Associations { get; set; } = new();

    public object Clone()
    {
        var clone = (RootNode)MemberwiseClone();
        clone.Association = (AssociationRoot?)Association?.Clone();
        clone.Associations = Associations.Select(x => (AssociationRoot)x.Clone()).ToList();
        return clone;
    }
}