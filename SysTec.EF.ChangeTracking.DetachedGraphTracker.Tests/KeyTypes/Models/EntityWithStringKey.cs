using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

public class EntityWithStringKey
{
    [Key] public string Id { get; set; } = string.Empty;
}