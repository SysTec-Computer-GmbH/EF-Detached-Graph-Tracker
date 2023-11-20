using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

public class Offer : IdBase
{
    public OfferHead Head { get; set; }

    public Customer? Customer { get; set; }

    public Project? Project { get; set; }

    [ForceDeleteOnMissingEntries] public List<OfferPosition> Positions { get; set; } = new();

    [ForceAggregation] public List<Employee> Editors { get; set; } = new();

    [ForceAggregation] public Department? IssuingDepartment { get; set; }
}