using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

public abstract class OfferPosition : IdBase
{
    public string Discriminator { get; set; }

    public string Number { get; set; }

    public int? SectionId { get; set; }

    public ProjectSection? Section { get; set; }

    public List<OfferPosition> Children { get; set; } = new();
}