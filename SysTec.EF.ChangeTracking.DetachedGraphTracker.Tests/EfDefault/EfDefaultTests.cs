using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.SimpleGraph;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault;

public class EfDefaultTests : TestBase<EfDefaultDbContext>
{
    [Test]
    public async Task _01_ShowcaseRemoveMethod_ForOptionalRelationships()
    {
        // This is default behavior for 1-n relationships when the relationship is not further configured

        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleList();

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootNodeUpdate = await DataHelper.GetRootNodeWithOptionalSimpleListFromDbWithIncludesAsync(dbContext);
            rootNodeUpdate.ListItems.Remove(rootNodeUpdate.ListItems[0]);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithOptionalSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(2));
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var optionalListItems = await dbContext.Set<OptionalListItem>().ToListAsync();
            Assert.That(optionalListItems, Has.Count.EqualTo(3));
        }
    }

    [Test]
    public async Task _02_ShowcaseRemoveMethod_ForRequiredRelationships()
    {
        // This is default behavior for 1-n relationships when the relationship is not further configured

        var rootNode = DataHelper.GetRootNodeWithRequiredSimpleList();

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootNodeUpdate = await DataHelper.GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(dbContext);
            rootNodeUpdate.ListItems.Remove(rootNodeUpdate.ListItems[0]);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(2));
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var requiredListItems = await dbContext.Set<RequiredListItem>().ToListAsync();
            Assert.That(requiredListItems, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public async Task _03_ShowcaseSetNull_OnReferenceNavigation_ForRequiredOneToManyRelationship()
    {
        var item = new EfDefaultItemWithRequiredRelationship
        {
            RequiredRelationship = new EfDefaultItem
            {
                Text = "Required Relationship"
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(item);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var itemFromDb = await dbContext.DefaultItemsWithRequiredRelationship.Include(i => i.RequiredRelationship)
                .SingleAsync(i => item.Id == i.Id);
            itemFromDb.RequiredRelationship = null;
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            // Item is dependent on the relationship. Therefore the item is deleted.
            var itemsFromDb = await dbContext.DefaultItemsWithRequiredRelationship.ToListAsync();
            Assert.That(itemsFromDb, Is.Empty);
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var requiredRelationships = await dbContext.Set<EfDefaultItem>().ToListAsync();
            Assert.That(requiredRelationships, Has.Count.EqualTo(1));
        }
    }

    [Test]
    public async Task _04_ShowcaseSetNull_OnReferenceNavigation_ForRequiredOneToOneRelationship()
    {
        var oneItem = new EfDefaultOneItem
        {
            Relationship = new EfDefaultOneItemTwo()
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(oneItem);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var oneItemFromDb = await dbContext.Set<EfDefaultOneItem>().Include(i => i.Relationship)
                .SingleAsync(i => i.Id == oneItem.Id);
            oneItemFromDb.Relationship = null;
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var oneItemsFromDb = await dbContext.Set<EfDefaultOneItemTwo>().ToListAsync();
            Assert.That(oneItemsFromDb, Is.Empty);
        }
    }

    [Test]
    public async Task _05_ShowcaseSelfReferencingItemsRelationships()
    {
        var rootNode = new EfDefaultRootNode
        {
            Text = "RootNode",
            Items = new List<EfDefaultSelfReferencingListItem>
            {
                new()
                {
                    Text = "1",
                    Children = new List<EfDefaultSelfReferencingListItem>
                    {
                        new()
                        {
                            Text = "1.1",
                            Children = new List<EfDefaultSelfReferencingListItem>
                            {
                                new()
                                {
                                    Text = "1.1.1",
                                    Children = new List<EfDefaultSelfReferencingListItem>
                                    {
                                        new()
                                        {
                                            Text = "1.1.1.1"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(rootNode);
            foreach (var item in rootNode.Items) SetRootNodeReference(item, rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootItemFromDb = await dbContext.Set<EfDefaultRootNode>().Include(i => i.Items)
                .SingleAsync(i => i.Id == rootNode.Id);
            RemoveItemsToGetHierarchy(rootItemFromDb);
            Assert.That(rootItemFromDb.Items, Has.Count.EqualTo(1));
            Assert.That(rootItemFromDb.Items[0].Children[0].Children, Has.Count.EqualTo(1));
            Assert.That(rootItemFromDb.Items[0].Children[0].Children[0].Children, Has.Count.EqualTo(1));
            Assert.That(rootItemFromDb.Items[0].Children[0].Children[0].Children[0].Children, Is.Empty);
        }
    }

    [Test]
    public async Task _06_ShowcaseSetValuesMethod_ForNestedEntities()
    {
        var rootNode = new EfDefaultRootNode
        {
            Text = "Root Init"
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = new EfDefaultRootNode
        {
            Id = rootNode.Id,
            Text = "Root Update",
            Items = new List<EfDefaultSelfReferencingListItem>
            {
                new()
                {
                    Text = "Item 1",
                    Children = new List<EfDefaultSelfReferencingListItem>
                    {
                        new()
                        {
                            Text = "Item 1.1"
                        }
                    }
                }
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootNodeFromDb = await dbContext.Set<EfDefaultRootNode>().SingleAsync(i => i.Id == rootNode.Id);

            dbContext.Entry(rootNodeFromDb).CurrentValues.SetValues(rootNodeUpdate);
            foreach (var collectionEntry in dbContext.Entry(rootNodeFromDb).Collections)
                collectionEntry.CurrentValue = rootNodeUpdate.Items;

            foreach (var item in rootNodeFromDb.Items) SetRootNodeReference(item, rootNodeFromDb);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootNodeFromDb = await dbContext.Set<EfDefaultRootNode>().Include(i => i.Items)
                .SingleAsync(i => i.Id == rootNode.Id);

            RemoveItemsToGetHierarchy(rootNodeFromDb);
            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo("Root Update"));
                Assert.That(rootNodeFromDb.Items, Has.Count.EqualTo(1));
            });
            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Items[0].Text, Is.EqualTo("Item 1"));
                Assert.That(rootNodeFromDb.Items[0].Children, Has.Count.EqualTo(1));
            });
            Assert.That(rootNodeFromDb.Items[0].Children[0].Text, Is.EqualTo("Item 1.1"));
        }
    }

    [Test]
    public async Task _07_ShowcaseDiscriminatorsAreSet_ForTphStrategy()
    {
        var entity = new EfDefaultSubType
        {
            Text = "New SubType"
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var entityFromDb = await dbContext.EfDefaultBaseTypes.SingleAsync(i => i.Id == entity.Id);
            Assert.That(entityFromDb.Discriminator, Is.EqualTo(nameof(EfDefaultSubType)));
        }
    }

    [Test]
    public async Task _08_ShowcaseNavigationProperty_IsPreferredOverForeignKey_ForRelationship()
    {
        await ShowcaseNavigationProperty_IsPreferredOverForeignKey_ForRelationship(
            _08_PerformChangeTrackingWithUpdateMethod);
    }

    [Test]
    public async Task _08_01_ShowcaseNavigationProperty_IsPreferredOverForeignKey_ForRelationship_WithGraphTracker()
    {
        await ShowcaseNavigationProperty_IsPreferredOverForeignKey_ForRelationship(
            _08_01_PerformChangeTrackingWithGraphTracker);
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _09_ShowcaseSeveringOfRelationships_ForOptionalRelationships()
    {
        var item = new EfDefaultItemWithOptionalRelationship
        {
            Text = "Text",
            OptionalRelationship = new EfDefaultItem
            {
                Text = "Optional Relationship"
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(item);
            await dbContext.SaveChangesAsync();
        }

        var itemUpdate = new EfDefaultItemWithOptionalRelationship
        {
            Id = item.Id,
            Text = "Text",
            OptionalRelationship = new EfDefaultItem
            {
                Id = item.OptionalRelationship.Id,
                Text = "Optional Relationship"
            }
        };

        itemUpdate.OptionalRelationship = null;

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Update(itemUpdate);
            Assert.Warn(
                "This is unexpected behavior. The Reference Navigation with a null value must be set to modified manually!" +
                "Another option would be to set the value to null, after the entity is attached to the context (not possible in a detached api scenario).");
            dbContext.Entry(itemUpdate).Reference(i => i.OptionalRelationship).IsModified = true;
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var itemFromDb = await dbContext.Set<EfDefaultItemWithOptionalRelationship>()
                .Include(i => i.OptionalRelationship)
                .SingleAsync(i => i.Id == itemUpdate.Id);

            Assert.That(itemFromDb.OptionalRelationship, Is.Null);
        }
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _10_ShowcaseCollectionReferenceLoadMethod_WillNotPopulateCollection_WithAlreadyTrackedItems()
    {
        var defaultItem = new EfDefaultItem
        {
            Text = "Item"
        };

        var root = new EfDefaultRootWithCollection
        {
            Text = "Root",
            Items = new List<EfDefaultItem>
            {
                defaultItem
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        // Root without the collection. The Collection is empty.
        var rootUpdate = new EfDefaultRootWithCollection
        {
            Id = root.Id,
            Text = "Root"
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            // Item is tracked
            dbContext.Entry(defaultItem).State = EntityState.Modified;

            var rootUpdateEntry = dbContext.Entry(rootUpdate);
            rootUpdateEntry.State = EntityState.Modified;

            Assert.That(rootUpdateEntry.Collection(r => r.Items).CurrentValue!.Count(), Is.EqualTo(0));

            Assert.Warn(
                "Although the description of the load method states, that already tracked items will" +
                " NOT BE OVERWRITTEN with new values from the database, i would assume that the already tracked items" +
                " would be present in the list after the load call.");

            await dbContext.Entry(rootUpdate).Collection(r => r.Items).LoadAsync();

            // For our project we would expect that the collection contains the tracked item.
            Assert.That(rootUpdateEntry.Collection(r => r.Items).CurrentValue!.Count(), Is.EqualTo(0));
        }
    }

    [Test]
    public async Task _11_ShowcaseMultipleSameOwnedTypes_InGraph_AreAlwaysDifferentInstances()
    {
        var root = new EfDefaultRootWithOwnedType
        {
            Text = "Root",
            A_OwnedEntity = new EfDefaultOwnedType
            {
                Text = "Owned Type"
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootFromDb = await dbContext.Set<EfDefaultRootWithOwnedType>()
                .Include(r => r.A_OwnedEntity)
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.A_OwnedEntity, Is.Not.Null);
        }

        var rootUpdate = (EfDefaultRootWithOwnedType)root.Clone();
        rootUpdate.A_OwnedEntity!.Text = "Updated Owned Type";
        rootUpdate.B_OwnedEntities.Add((EfDefaultOwnedType)rootUpdate.A_OwnedEntity.Clone());

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Update(rootUpdate);
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContext.SaveChangesAsync());
        }

        // await using (var dbContext = new EfDefaultDbContext())
        // {
        //     var rootFromDb = await dbContext.Set<EfDefaultRootWithOwnedType>()
        //         .Include(r => r.A_OwnedEntity)
        //         .SingleAsync(r => r.Id == root.Id);
        //     
        //     Assert.That(rootFromDb.A_OwnedEntity, Is.Not.Null); 
        //     Assert.That(rootFromDb.B_OwnedEntities, Has.Count.EqualTo(1));
        //     Assert.That(rootFromDb.A_OwnedEntity, Is.Not.SameAs(rootFromDb.B_OwnedEntities[0]));
        // }

        Assert.Ignore(
            "This test throws in the save changes call. The issue is not further investigated at the moment " +
            "because support for owned collections is not mandatory for our use case.");
    }

    [Test]
    public async Task _12_ShowcaseCRUD_OfOwnedCollectionTypes()
    {
        var root = new EfDefaultRootWithOwnedType
        {
            Text = "Root",
            B_OwnedEntities = new List<EfDefaultOwnedType>
            {
                new()
                {
                    Text = "Owned Type 1"
                },
                new()
                {
                    Text = "Owned Type 2"
                }
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootFromDb = await dbContext.Set<EfDefaultRootWithOwnedType>()
                .Include(r => r.B_OwnedEntities)
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.B_OwnedEntities, Has.Count.EqualTo(2));
        }

        var rootUpdate = (EfDefaultRootWithOwnedType)root.Clone();
        rootUpdate.B_OwnedEntities[0].Text = "Updated Owned Type 1";

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Update(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootFromDb = await dbContext.Set<EfDefaultRootWithOwnedType>()
                .Include(r => r.B_OwnedEntities)
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.B_OwnedEntities, Has.Count.EqualTo(2));
            Assert.That(rootFromDb.B_OwnedEntities[0].Text, Is.EqualTo(rootUpdate.B_OwnedEntities[0].Text));
        }

        var rootUpdate2 = (EfDefaultRootWithOwnedType)root.Clone();
        rootUpdate2.B_OwnedEntities.RemoveAt(0);

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Update(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var rootFromDb = await dbContext.Set<EfDefaultRootWithOwnedType>()
                .Include(r => r.B_OwnedEntities)
                .SingleAsync(r => r.Id == root.Id);

            // Same behavior as in normal collections. In disconnected scenarios, no deletes are made.
            Assert.That(rootFromDb.B_OwnedEntities, Has.Count.EqualTo(2));
        }
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _13_ShowcaseAttachMethod_WithSubtreeAlreadyTracked()
    {
        var root = new EfDefaultRootWithReference
        {
            Item = new EfDefaultItem
            {
                Text = "Item"
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            // Save test data for the testcase
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var itemToAttach = new EfDefaultItem
        {
            Id = root.Item.Id
        };

        var rootToAttach = new EfDefaultRootWithReference
        {
            Id = root.Id,
            Item = new EfDefaultItem
            {
                Id = root.Item.Id
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            // First attach EfDefaultItem {Id: 1}
            dbContext.Attach(itemToAttach);

            // Then attach EfDefaultRootWithReference {Id: 1, Item(EfDefaultItem) {Id: 1}}
            Assert.Throws<InvalidOperationException>(() => dbContext.Attach(rootToAttach));
        }

        // According to the microsoft documentation:
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext.attach?view=efcore-7.0 (See section remarks)
        // A recursive search of the navigation properties will be performed to find reachable entities
        // that ARE NOT ALREADY TRACKED by the context. All entities found will be tracked by the context.

        // Expectation:
        // The second call to attach should not throw, because EfDefaultItem {Id: 1} is already tracked but should be
        // ignored by the attach method like stated in the documentation.
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _14_ShowcaseRestrictDeleteBehavior_ForNonRequiredRelationship_WithIncludedDependentBackreference()
    {
        // https://github.com/dotnet/efcore/issues/26857
        var item = new EfDefaultItemWithOptionalRelationship
        {
            Text = "Dependent Item",
            OptionalRelationship = new EfDefaultItem
            {
                Text = "Principal Item"
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(item);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var itemFromDb = await dbContext.Set<EfDefaultItemWithOptionalRelationship>()
                .Include(i => i.OptionalRelationship)
                .SingleAsync(i => i.Id == item.Id);

            Assert.That(itemFromDb.OptionalRelationship, Is.Not.Null);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            // Useless behavior, the configured delete behavior is ignored, when the dependent backreference is included
            var itemFromDb = await dbContext.Set<EfDefaultItem>()
                .Include(e => e.Backreferences)
                .SingleAsync(i => i.Id == item.OptionalRelationship.Id);

            dbContext.Remove(itemFromDb);
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var itemFromDb = await dbContext.Set<EfDefaultItem>()
                .SingleOrDefaultAsync(i => i.Id == item.OptionalRelationship.Id);

            Assert.That(itemFromDb, Is.Null);
        }
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _15_ShowcaseExplicitLoad_OfManyToManyCollection_ThrowsError()
    {
        var manyItem = new EfDefaultManyItemOne
        {
            Items = new List<EfDefaultManyItemTwo>
            {
                new EfDefaultManyItemTwo()
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var manyItemFromDb = await dbContext.ManyItems.SingleAsync(i => i.Id == manyItem.Id);
            var query = dbContext.Entry(manyItemFromDb).Collection(s => s.Items).Query();
            // WHY THE FUCK DOES LOADING A M:N COLLECTION NAVIGATION CAUSE A NULL REFERENCE EXCEPTION WHEN USING SPLIT QUERY???
            Assert.ThrowsAsync<NullReferenceException>(async () => await query.AsSplitQuery().ToListAsync());
        }
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _16_ShowcaseUpdatingItemInManyToManyRelationship_AlwaysSetsBindEntryToAdded()
    {
        var manyItem = new EfDefaultManyItemOne
        {
            Items = new List<EfDefaultManyItemTwo>
            {
                new EfDefaultManyItemTwo()
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new EfDefaultManyItemOne
        {
            Id = manyItem.Id,
            Items = new List<EfDefaultManyItemTwo>
            {
                new EfDefaultManyItemTwo
                {
                    Id = manyItem.Items[0].Id
                }
            }
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Update(manyItemUpdate);
            var bindEntityEntry = dbContext.ChangeTracker.Entries()
                .Single(e =>
                    e.Entity.GetType() != typeof(EfDefaultManyItemOne) &&
                    e.Entity.GetType() != typeof(EfDefaultManyItemTwo));
            Assert.That(bindEntityEntry.State, Is.EqualTo(EntityState.Added));

            // Call throws because the bind entry is set to added
            Assert.ThrowsAsync<DbUpdateException>(async () => await dbContext.SaveChangesAsync());
        }
    }

    [Test]
    [Category("Unexpected Behavior")]
    public async Task _17_ShowcaseUpdate_OfEntryWith_GeneratedValueAlways_ThrowsError()
    {
        var item = new EfDefaultItemWithGeneratedValue();

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(item);
            await dbContext.SaveChangesAsync();
        }

        var itemUpdate = new EfDefaultItemWithGeneratedValue
        {
            Id = item.Id,
            GeneratedValue = item.GeneratedValue
        };

        // Simulate detached scenario
        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Entry(itemUpdate).State = EntityState.Modified;
            var generatedValueProperty = dbContext.Entry(itemUpdate).Property(i => i.GeneratedValue);

            Assert.That(generatedValueProperty.IsModified, Is.True);
            Assert.ThrowsAsync<DbUpdateException>(async () => await dbContext.SaveChangesAsync());
        }

        // Even with the following non detached scenario, the save after update throws!
        await using (var dbContext = new EfDefaultDbContext())
        {
            var itemFromDb = await dbContext.Set<EfDefaultItemWithGeneratedValue>()
                .SingleAsync();

            dbContext.Update(itemFromDb);
            var generatedValueProperty = dbContext.Entry(itemFromDb).Property(i => i.GeneratedValue);

            // Even though the change tracker tracks the item from the beginning, the default update method also sets generated value properties to modified.
            Assert.That(generatedValueProperty.IsModified, Is.True);
            Assert.ThrowsAsync<DbUpdateException>(async () => await dbContext.SaveChangesAsync());
        }
    }

    [Test]
    public async Task _18_Showcase_Backreferences_AreSetAutomatically_ForOneToManyRelationship()
    {
        var root = new EfDefaultManyRoot()
        {
            Items = new()
            {
                new()
            }
        };
        
        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }
        
        var rootUpdate = new EfDefaultManyRoot()
        {
            Id = root.Id,
            Items = new()
            {
                new()
                {
                    Id = root.Items.First().Id
                }
            }
        };
        
        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Update(rootUpdate);
        }
        
        Assert.That(rootUpdate, Is.SameAs(rootUpdate.Items.First().Root));
    }

    [Test]
    public async Task _19_Showcase_UpdateValuesInConditionalUniqueConstraint_WithSingleSaveChangesCall()
    {
        var itemOne = new EfDefaultItemWithUniqueConstraint()
        {
            UniqueConstraintWhenTrue = true
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(itemOne);
            await dbContext.SaveChangesAsync();
        }
        
        var itemTwo = new EfDefaultItemWithUniqueConstraint()
        {
            UniqueConstraintWhenTrue = true
        };
        
        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(itemTwo);
            var itemOneFromDb = await dbContext.Set<EfDefaultItemWithUniqueConstraint>()
                .SingleAsync(i => i.Id == itemOne.Id);
            itemOneFromDb.UniqueConstraintWhenTrue = false;
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }
    }

    private async Task ShowcaseNavigationProperty_IsPreferredOverForeignKey_ForRelationship(
        PerformChangeTracking performChangeTracking)
    {
        const int notExistingId = 1234;

        var relationshipOneSide = new EfDefaultItem
        {
            Text = "Many Side"
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.Add(relationshipOneSide);
            await dbContext.SaveChangesAsync();
        }

        var relationshipManySide = new EfDefaultRelationshipManySide
        {
            Text = "One Side",
            RelationshipId = notExistingId,
            Relationship = relationshipOneSide
        };

        await using (var dbContext = new EfDefaultDbContext())
        {
            performChangeTracking(dbContext, relationshipManySide);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var relationshipManySideFromDb = await dbContext.Set<EfDefaultRelationshipManySide>()
                .Include(i => i.Relationship)
                .SingleAsync(i => i.Id == relationshipManySide.Id);

            Assert.Multiple(() =>
            {
                Assert.That(relationshipManySideFromDb.RelationshipId, Is.EqualTo(relationshipOneSide.Id));
                Assert.That(relationshipManySideFromDb.Relationship, Is.Not.Null);
            });
        }

        await using (var dbContext = new EfDefaultDbContext())
        {
            var dbRelationshipsOneSide = await dbContext.Set<EfDefaultItem>()
                .ToListAsync();

            Assert.That(dbRelationshipsOneSide, Has.Count.EqualTo(1));
        }
    }

    private static void _08_PerformChangeTrackingWithUpdateMethod(DbContext dbContext,
        EfDefaultRelationshipManySide entity)
    {
        dbContext.Update(entity);
    }

    private void _08_01_PerformChangeTrackingWithGraphTracker(DbContext dbContext, EfDefaultRelationshipManySide entity)
    {
        var graphTracker = GetGraphTrackerInstance(dbContext);
        graphTracker.TrackGraphAsync(entity).Wait();
    }

    private void SetRootNodeReference(EfDefaultSelfReferencingListItem item, EfDefaultRootNode rootNode)
    {
        item.EfDefaultRootNode = rootNode;
        foreach (var child in item.Children) SetRootNodeReference(child, rootNode);
    }

    private void RemoveItemsToGetHierarchy(EfDefaultRootNode rootNode)
    {
        rootNode.Items.RemoveAll(i => i.Parent != null);
    }

    private delegate void PerformChangeTracking(DbContext dbContext, EfDefaultRelationshipManySide entity);
}