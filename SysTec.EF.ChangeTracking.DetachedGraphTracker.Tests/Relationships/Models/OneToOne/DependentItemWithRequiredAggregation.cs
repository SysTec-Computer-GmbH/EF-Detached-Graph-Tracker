using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

public class DependentItemWithRequiredAssociation : IdBase
{
    [UpdateAssociationOnly] public PrincipalItemForAssociation RequiredPrincipal { get; set; }
}