using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToOneRelationship;

public class OptionalOneItemOne : IdBase
{
    public int? OptionalItemId { get; set; }
    
    public OptionalOneItemTwo OptionalItem { get; set; }
}