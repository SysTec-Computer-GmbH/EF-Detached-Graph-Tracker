using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Reference;

[Table("ForceRelationshipUnchangedReferenceAbstractTypes")]
public abstract class AbstractTypeWithAbstractReference : IdBase
{
    public abstract int? ItemId { get; set; }
    
    public abstract ReferenceItemWithBackreferenceToAbstractType Item { get; set; }
}