using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

public class PrincipalItemForComposition : IdBase
{
    public DependentItemWithRequiredComposition? OptionalDependent { get; set; }
}