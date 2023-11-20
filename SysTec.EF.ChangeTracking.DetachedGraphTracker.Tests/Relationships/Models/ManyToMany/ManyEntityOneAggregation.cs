using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;

public class ManyEntityOneAggregation : IdBase
{
    public string? Text { get; set; }

    [ForceAggregation] public List<ManyEntityTwoAggregation> ManyAggregations { get; set; } = new();
}