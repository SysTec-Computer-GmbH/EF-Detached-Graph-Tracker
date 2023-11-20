using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations.Models;

public class NodeWithoutKey
{
    [Key] public string NullableKey { get; set; }
}