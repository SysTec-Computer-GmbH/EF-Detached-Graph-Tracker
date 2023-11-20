using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

public class RootNode : IdBase
{
    public Entity? Composition { get; set; }

    public string? Text { get; set; }

    [UpdateAssociationOnly] public List<Entity> Associations { get; set; } = new();
}