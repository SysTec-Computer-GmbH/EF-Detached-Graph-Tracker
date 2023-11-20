using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

public class PrincipalItemForAssociation : IdBase
{
    public DependentItemWithRequiredAssociation? OptionalDependent { get; set; }
}