using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;

public class ManyEntityOneAssociation : IdBase
{
    public string? Text { get; set; }

    [UpdateAssociationOnly] public List<ManyEntityTwoAssociation> ManyAssociations { get; set; } = new();
}