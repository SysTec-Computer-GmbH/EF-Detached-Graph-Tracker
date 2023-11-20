using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships;

public class SelfReferencingOneToManyTests : TestBase<RelationshipTestsDbContext>
{
    [Test]
    public async Task _01_SelfReferencingOneToManyRelationship_CanBeCreated_AndSevered_FromPrincipalSide()
    {
        var root = new SelfReferencingItem
        {
            Text = "Root",
            Children = new List<SelfReferencingItem>
            {
                new()
                {
                    Text = "Child 1",
                    Children = new List<SelfReferencingItem>
                    {
                        new()
                        {
                            Text = "Child 1.1",
                            Children = new List<SelfReferencingItem>
                            {
                                new()
                                {
                                    Text = "Child 1.1.1"
                                }
                            }
                        },
                        new()
                        {
                            Text = "Child 1.2"
                        }
                    }
                },
                new()
                {
                    Text = "Child 2",
                    Children = new List<SelfReferencingItem>
                    {
                        new SelfReferencingItem
                        {
                            Text = "Child 2.1"
                        }
                    }
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var rootFromDb = await dbContext.SelfReferencingItems
                .Include(i => i.Children)
                .ThenInclude(i => i.Children)
                .ThenInclude(i => i.Children)
                .ThenInclude(i => i.Children)
                .SingleAsync(i => i.Id == root.Id);

            Assert.That(rootFromDb.Children, Has.Count.EqualTo(2));
            Assert.That(rootFromDb.Children[0].Children, Has.Count.EqualTo(2));
            Assert.That(rootFromDb.Children[0].Children[0].Children, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.Children[1].Children, Has.Count.EqualTo(1));
        }

        var rootUpdate = (SelfReferencingItem)root.Clone();
        // Remove Child 1.1.1
        rootUpdate.Children[0].Children[0].Children = null;

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var rootFromDb = await dbContext.SelfReferencingItems
                .Include(i => i.Children)
                .ThenInclude(i => i.Children)
                .ThenInclude(i => i.Children)
                .ThenInclude(i => i.Children)
                .SingleAsync(i => i.Id == root.Id);

            Assert.That(rootFromDb.Children, Has.Count.EqualTo(2));
            Assert.That(rootFromDb.Children[0].Children, Has.Count.EqualTo(2));
            Assert.That(rootFromDb.Children[0].Children[0].Children, Is.Empty);
            Assert.That(rootFromDb.Children[1].Children, Has.Count.EqualTo(1));
        }
    }
}