using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;

public class RootNode : IdBase
{
    public List<ManyEntityTwo> A_Compositions { get; set; } = new();

    public ManyEntityOne B_Composition { get; set; }
}