using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultItemWithRequiredRelationship : IdBase
{
    public int RequiredRelationshipId { get; set; }
    public EfDefaultItem RequiredRelationship { get; set; }
}