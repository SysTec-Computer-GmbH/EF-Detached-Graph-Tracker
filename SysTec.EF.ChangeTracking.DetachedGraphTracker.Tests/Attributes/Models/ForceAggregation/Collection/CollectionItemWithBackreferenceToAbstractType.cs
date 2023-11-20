using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceAggregation.Collection;

[Table("ForceAggregationCollectionItems")]
public class CollectionItemWithBackreferenceToAbstractType : IdBase
{
    public int AbstractTypeId { get; set; }

    public AbstractTypeWithAbstractCollection AbstractType { get; set; }

    public string? Text { get; set; }
}