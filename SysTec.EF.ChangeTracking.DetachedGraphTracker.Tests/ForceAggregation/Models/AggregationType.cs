using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models;

public class AggregationType : IdBase
{
    public string Text { get; set; }
}