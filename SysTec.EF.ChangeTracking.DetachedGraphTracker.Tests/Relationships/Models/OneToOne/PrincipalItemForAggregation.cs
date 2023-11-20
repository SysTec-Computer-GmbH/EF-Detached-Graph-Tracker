using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

public class PrincipalItemForAggregation : IdBase
{
    public DependentItemWithRequiredAggregation? OptionalDependent { get; set; }
}