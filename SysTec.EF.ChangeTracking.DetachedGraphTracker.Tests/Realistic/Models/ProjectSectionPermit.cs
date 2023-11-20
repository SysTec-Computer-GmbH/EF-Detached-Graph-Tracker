using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

public class ProjectSectionPermit : IdBase
{
    public string Name { get; set; }

    [ForceAggregation] public List<ProjectSection> Sections { get; set; } = new();
}