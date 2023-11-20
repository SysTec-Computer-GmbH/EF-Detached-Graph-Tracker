using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

public class DependentManyItemWithDefinedFkOptionalAggregation : IdBase
{
    public int? OptionalAggregationId { get; set; }

    [UpdateAssociationOnly] public OneItem? OptionalAggregation { get; set; }
}