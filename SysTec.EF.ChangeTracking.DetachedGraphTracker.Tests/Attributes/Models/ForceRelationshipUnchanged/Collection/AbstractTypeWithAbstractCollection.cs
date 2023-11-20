using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Collection;

[Table("ForceRelationshipUnchangedAbstractTypes")]
public abstract class AbstractTypeWithAbstractCollection : IdBase
{
    public abstract List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; }
}