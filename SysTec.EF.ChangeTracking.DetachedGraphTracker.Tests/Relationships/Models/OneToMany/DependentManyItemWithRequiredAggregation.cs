using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

public class DependentManyItemWithRequiredAggregation : IdBase
{
    [ForceAggregation] public OneItem RequiredAggregation { get; set; }
}