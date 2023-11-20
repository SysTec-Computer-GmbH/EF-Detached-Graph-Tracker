using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Models;

public class RootWithConcurrencyItemInForceAggregationReference : IdBase
{
    [UpdateAssociationOnly]
    public ItemWithConcurrencyStamp? ItemWithConcurrencyStamp { get; set; }
}