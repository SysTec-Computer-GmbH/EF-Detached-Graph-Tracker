using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultOneItemTwo : IdBase
{
    public int RelationshipId { get; set; }
    public EfDefaultOneItem Relationship { get; set; }
}