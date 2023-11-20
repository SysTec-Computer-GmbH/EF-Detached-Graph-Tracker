using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceAggregation.Collection;

[Table("ForceAggregationAbstractTypes")]
public abstract class AbstractTypeWithAbstractCollection : IdBase
{
    public abstract List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; }
}