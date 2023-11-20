using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations.Models;

public class NodeWithInvalidKeyType
{
    [Key] public string InvalidKey { get; set; }
}