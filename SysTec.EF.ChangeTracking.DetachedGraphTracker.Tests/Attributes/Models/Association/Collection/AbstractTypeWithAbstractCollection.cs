using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.Association.Collection;

[Table("AssociationAbstractTypes")]
public abstract class AbstractTypeWithAbstractCollection : IdBase
{
    public abstract List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; }
}