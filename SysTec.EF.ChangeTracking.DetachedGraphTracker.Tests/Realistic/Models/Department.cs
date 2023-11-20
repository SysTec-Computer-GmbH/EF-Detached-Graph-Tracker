using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

public class Department : IdBase
{
    public string Name { get; set; }

    public Company Company { get; set; }
}