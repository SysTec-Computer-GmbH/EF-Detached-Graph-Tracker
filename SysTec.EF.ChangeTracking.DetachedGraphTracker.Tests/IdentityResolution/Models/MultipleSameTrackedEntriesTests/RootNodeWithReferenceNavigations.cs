using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

public class RootNodeWithReferenceNavigations : IdBase
{
    [UpdateAssociationOnly] 
    public Entity A_Aggregation { get; set; }

    public Entity B_Composition { get; set; }
    
    [UpdateAssociationOnly]
    public Entity C_Aggregation { get; set; }
}