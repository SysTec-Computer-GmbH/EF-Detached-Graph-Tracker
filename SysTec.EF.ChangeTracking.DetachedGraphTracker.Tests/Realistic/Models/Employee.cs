using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

public class Employee : IdBase
{
    public string Name { get; set; }

    public List<City>? Cities { get; set; }
}