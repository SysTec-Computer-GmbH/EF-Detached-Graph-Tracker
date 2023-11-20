using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;

public class ManyEntityOne : IdBase
{
    public string? Text { get; set; }

    public List<ManyEntityTwo> ManyCompositions { get; set; } = new();
}