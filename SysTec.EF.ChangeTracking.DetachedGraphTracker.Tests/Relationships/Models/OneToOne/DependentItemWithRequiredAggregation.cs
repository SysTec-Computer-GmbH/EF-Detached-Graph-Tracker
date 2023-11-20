using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

public class DependentItemWithRequiredAggregation : IdBase
{
    [ForceAggregation] public PrincipalItemForAggregation RequiredPrincipal { get; set; }
}