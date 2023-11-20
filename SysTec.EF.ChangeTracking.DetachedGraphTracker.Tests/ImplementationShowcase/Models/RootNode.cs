using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

public class RootNode : IdBase
{
    public Entity? Composition { get; set; }

    //[ForceAggregation]
    //public Entity? Aggregation { get; set; }

    //public List<Entity> Compositions { get; set; } = new();

    public string? Text { get; set; }

    [UpdateAssociationOnly] public List<Entity> Aggregations { get; set; } = new();
}