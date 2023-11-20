using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

public class EntityWithByteArrayKey
{
    [Key] public byte[] Id { get; set; } = Array.Empty<byte>();
}