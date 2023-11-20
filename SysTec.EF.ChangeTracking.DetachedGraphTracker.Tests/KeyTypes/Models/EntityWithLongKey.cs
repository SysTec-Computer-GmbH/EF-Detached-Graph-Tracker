using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

public class EntityWithLongKey
{
    [Key]
    public long Id { get; set; }
}