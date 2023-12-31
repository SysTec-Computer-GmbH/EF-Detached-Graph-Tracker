using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

public class EntityWithPropertiesNewInitializer : IdBase
{
    public int? CompositionEntityId { get; set; }

    public Entity? CompositionEntity { get; set; } = new();

    public int? AssociationEntityId { get; set; }

    [UpdateAssociationOnly] public Entity? AssociationEntity { get; set; } = new();

    public string Text { get; set; }
}