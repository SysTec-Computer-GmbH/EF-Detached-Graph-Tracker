using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

public class DependentManyItemWithOptionalAssociation : IdBase
{
    [UpdateAssociationOnly] public OneItem? OptionalAssociation { get; set; }
}