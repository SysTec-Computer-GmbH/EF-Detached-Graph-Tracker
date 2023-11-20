using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

public class DependentManyItemWithDefinedFkOptionalComposition : IdBase
{
    public int? OptionalCompositionId { get; set; }

    public OneItem? OptionalComposition { get; set; }
}