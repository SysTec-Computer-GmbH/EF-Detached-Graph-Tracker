using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;

public class ManyEntityTwo : IdBase
{
    public string? Text { get; set; }

    [UpdateAssociationOnly] public List<ManyEntityOne> EntityOneAssociations { get; set; } = new();
}