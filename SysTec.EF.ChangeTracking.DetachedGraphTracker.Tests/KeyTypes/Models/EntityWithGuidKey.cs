using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

public class EntityWithGuidKey
{
    [Key]
    public Guid Id { get; set; }
}