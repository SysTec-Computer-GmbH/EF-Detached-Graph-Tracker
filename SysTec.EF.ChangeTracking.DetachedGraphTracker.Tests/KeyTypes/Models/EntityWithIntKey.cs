using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

public class EntityWithIntKey
{
    [Key]
    public int Id { get; set; }
}