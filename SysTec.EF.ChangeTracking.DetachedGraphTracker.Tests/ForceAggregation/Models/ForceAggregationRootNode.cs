using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models;

public class ForceAggregationRootNode : IdBase
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public AggregationType? AggregationTypeReference { get; set; }

    [UpdateAssociationOnly] public List<AggregationType> AggregationTypeCollection { get; set; } = new();
}