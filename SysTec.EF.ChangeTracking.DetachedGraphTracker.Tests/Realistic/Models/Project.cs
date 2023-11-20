using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

public class Project : IdBase
{
    public string Name { get; set; }

    [ForceAggregation] public Customer? Customer { get; set; }

    public List<ProjectSection> Sections { get; set; } = new();

    public List<ProjectSectionPermit> SectionPermits { get; set; } = new();
}