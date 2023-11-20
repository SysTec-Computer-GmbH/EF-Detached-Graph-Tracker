using Microsoft.EntityFrameworkCore;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

[Owned]
public class OfferHead
{
    public string? PaymentCondition { get; set; }
}