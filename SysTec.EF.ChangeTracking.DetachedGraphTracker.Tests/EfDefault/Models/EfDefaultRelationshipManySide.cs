using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultRelationshipManySide : IdBase
{
    public string Text { get; set; }

    public int RelationshipId { get; set; }

    public EfDefaultItem Relationship { get; set; }
}