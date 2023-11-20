using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceAggregation.Reference;

[Table("ForceAggregationReferenceAbstractTypes")]
public abstract class AbstractTypeWithAbstractReference : IdBase
{
    public abstract int? ItemId { get; set; }
    
    public abstract ReferenceItemWithBackreferenceToAbstractType Item { get; set; }
}