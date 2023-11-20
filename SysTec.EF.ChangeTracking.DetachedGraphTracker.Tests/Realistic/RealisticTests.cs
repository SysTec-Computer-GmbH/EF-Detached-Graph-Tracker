using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic;

public class RealisticTests
{
    [SetUp]
    public async Task Setup()
    {
        var dbContext = new TestDbContext();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    [Test]
    public async Task NestedLists_AreUpdatedCorrectly()
    {
        var offer = new Offer
        {
            Head = new OfferHead
            {
                PaymentCondition = "30 Days"
            },
            Id = 1,
            Positions = new List<OfferPosition>
            {
                new HeaderPosition
                {
                    Id = 1,
                    Number = "1",
                    Heading = "Heading",
                    Children = new List<OfferPosition>
                    {
                        new BillablePosition
                        {
                            Id = 2,
                            Number = "1.1",
                            Activity = "Setup"
                        }
                    }
                },
                new BillablePosition
                {
                    Id = 3,
                    Number = "2",
                    Activity = "Teardown"
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            dbContext.Add(offer);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var positions = await dbContext.Positions.ToListAsync();
            Assert.That(positions, Has.Count.EqualTo(3));
        }

        const string updatedActivity = "Teardown changed";
        var offerUpdate = new Offer
        {
            Id = 1,
            Positions = new List<OfferPosition>
            {
                new BillablePosition
                {
                    Id = 3,
                    Number = "2",
                    Activity = updatedActivity
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offerUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var positions = await dbContext.Positions.ToListAsync();
            Assert.That(positions, Has.Count.EqualTo(1));
            Assert.That(((BillablePosition)positions.Single(p => p.Id == 3)).Activity,
                Is.EqualTo(updatedActivity));
        }
    }

    [Test]
    public async Task IdentityResolution_ForAssociationsInLists_AreWorking()
    {
        const string mainSectionName = "Main Section";
        var offer = new Offer
        {
            Head = new OfferHead
            {
                PaymentCondition = "30 Days"
            },
            Project = new Project
            {
                Name = "Project 1",
                Sections = new List<ProjectSection>
                {
                    new()
                    {
                        Name = mainSectionName
                    },
                    new()
                    {
                        Name = "Other Section"
                    }
                }
            },
            Positions = new List<OfferPosition>
            {
                new HeaderPosition
                {
                    Number = "1",
                    Heading = "Heading"
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offer);
            await dbContext.SaveChangesAsync();
        }


        var offerUpdate = new Offer
        {
            Id = 1,
            Project = new Project
            {
                Id = 1,
                Name = "Project 1",
                Sections = new List<ProjectSection>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Main Section"
                    }
                },
                SectionPermits = new List<ProjectSectionPermit>
                {
                    new()
                    {
                        Name = "Permit 1",
                        Sections = new List<ProjectSection>
                        {
                            new()
                            {
                                Id = 1,
                                Name = "This string should not be persisted"
                            }
                        }
                    }
                }
            },
            Positions = new List<OfferPosition>
            {
                new HeaderPosition
                {
                    Id = 1,
                    Number = "1",
                    Heading = "Heading",
                    SectionId = 1
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offerUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var position = await dbContext.Positions.Include(p => p.Section).FirstOrDefaultAsync();
            Assert.Multiple(() =>
            {
                Assert.That(position, Is.Not.Null);
                Assert.That(position!.SectionId, Is.EqualTo(1));
                Assert.That(position.Section, Is.Not.Null);
            });
        }

        await using (var dbContext = new TestDbContext())
        {
            var permit = await dbContext.Set<ProjectSectionPermit>()
                .Include(va => va.Sections).SingleAsync();
            
            Assert.That(permit.Sections, Has.Count.EqualTo(1));
            Assert.That(permit.Sections.Single().Name, Is.EqualTo(mainSectionName));
        }
    }

    [Test]
    public async Task ListsItems_AreDeletedCorrectly()
    {
        var offer = new Offer
        {
            Head = new OfferHead
            {
                PaymentCondition = "3 Weeks"
            },
            Positions = new List<OfferPosition>
            {
                new BillablePosition
                {
                    Number = "1",
                    Activity = "Doing nothing but billing it"
                },
                new HeaderPosition
                {
                    Number = "2",
                    Heading = "Heading"
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offer);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var positions = await dbContext.Positions.ToListAsync();
            Assert.That(positions, Has.Count.EqualTo(2));
        }

        const string headingUpdated = "Heading updated";
        const string numberUpdated = "1";
        var offerUpdate = new Offer
        {
            Id = 1,
            Positions = new List<OfferPosition>
            {
                new HeaderPosition
                {
                    Id = 2,
                    Number = numberUpdated,
                    Heading = headingUpdated
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offerUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var offerFromDb = await dbContext.Offers.Include(a => a.Positions)
                .SingleAsync(a => a.Id == offerUpdate.Id);
            
            Assert.That(offerFromDb.Positions, Has.Count.EqualTo(1));
            Assert.That(((offerFromDb.Positions.Single() as HeaderPosition)!).Heading, Is.EqualTo(headingUpdated));
            Assert.That(((offerFromDb.Positions.Single() as HeaderPosition)!).Number, Is.EqualTo(numberUpdated));
        }

        await using (var dbContext = new TestDbContext())
        {
            var positions = await dbContext.Positions.ToListAsync();
            Assert.That(positions, Has.Count.EqualTo(1));
        }
    }

    [Test]
    public async Task OwnedEntities_AreUpdatedCorrectly()
    {
        const string initPaymentCondition = "30 Days";
        const string updatedPaymentCondition = "14 Days";

        var offer = new Offer
        {
            Head = new OfferHead
            {
                PaymentCondition = initPaymentCondition
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offer);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var offerFromDb = await dbContext.Offers.SingleAsync();
            Assert.That(offerFromDb.Head.PaymentCondition, Is.EqualTo(initPaymentCondition));
            await dbContext.SaveChangesAsync();
        }

        var offerUpdate = new Offer
        {
            Id = 1,
            Head = new OfferHead
            {
                PaymentCondition = updatedPaymentCondition
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offerUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var offerFromDb = await dbContext.Offers.SingleAsync();
            Assert.That(offerFromDb.Head.PaymentCondition, Is.EqualTo(updatedPaymentCondition));
        }
    }

    [Test]
    public async Task Associations_ForLists_AreWorking()
    {
        const string nameJohnInit = "John";
        const string nameJohnUpdated = "John Doe";
        const string cityJohnInit = "Washington";
        const string cityJohnUpdated = "Washington D.C.";

        const string nameJaneInit = "Jane";
        const string nameJaneUpdated = "Jane Doe";

        var employeeJohn = new Employee
        {
            Name = nameJohnInit,
            Cities = new List<City>
            {
                new()
                {
                    Name = cityJohnInit
                }
            }
        };

        var employeeJane = new Employee
        {
            Name = nameJaneInit
        };

        await using (var dbContext = new TestDbContext())
        {
            dbContext.AddRange(employeeJohn, employeeJane);
            await dbContext.SaveChangesAsync();
        }

        var offer = new Offer
        {
            Editors = new List<Employee>
            {
                new()
                {
                    Id = employeeJohn.Id,
                    Name = nameJohnUpdated,
                    Cities = new List<City>
                    {
                        new()
                        {
                            Id = employeeJohn.Cities[0].Id,
                            Name = cityJohnUpdated
                        }
                    }
                },
                new()
                {
                    Id = employeeJane.Id,
                    Name = nameJaneUpdated
                },
                new()
                {
                    Name = "This should not be persisted because the list is marked as Association"
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            Assert.ThrowsAsync<AddedAssociationEntryException>(async () => await graphTracker.TrackGraphAsync(offer)); 
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var offerFromDb =
                await dbContext.Offers.Include(a => a.Editors).ThenInclude(b => b.Cities).SingleAsync();
            
            Assert.That(offerFromDb.Editors, Has.Count.EqualTo(2));
            
            var editors = offerFromDb.Editors;
            Assert.That(editors, Has.Count.EqualTo(2));
            
            var john = editors.Single(p => p.Id == employeeJohn.Id);
            Assert.That(john.Name, Is.EqualTo(nameJohnInit));
            Assert.That(john.Cities![0].Name, Is.EqualTo(cityJohnInit));
            
            Assert.That(editors.Single(p => p.Id == employeeJane.Id).Name, Is.EqualTo(nameJaneInit)); 
        }
    }

    [Test]
    public async Task Associations_ForNormalNavigations_AreWorking()
    {
        const string nameDepartmentInit = "Software Development";
        const string nameDepartmentUpdated = "Software Engineering";

        const string nameCompanyInit = "SysTec";
        const string nameCompanyUpdated = "SysTec Computer GmbH";

        var department = new Department
        {
            Name = nameDepartmentInit,
            Company = new Company
            {
                Name = nameCompanyInit
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            // EF-Default behavior: Everything is a composition.
            dbContext.Add(department);
            await dbContext.SaveChangesAsync();
        }

        var offer = new Offer
        {
            IssuingDepartment = new Department
            {
                Id = department.Id,
                Name = nameDepartmentUpdated,
                Company = new Company
                {
                    Id = department.Company.Id,
                    Name = nameCompanyUpdated
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offer);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var departmentFromDb = await dbContext.Set<Department>().Include(f => f.Company).SingleAsync();
            Assert.That(departmentFromDb.Name, Is.EqualTo(nameDepartmentInit));
            Assert.That(departmentFromDb.Company.Name, Is.EqualTo(nameCompanyInit));
        }
    }

    [Test]
    public async Task IdentityResolution_ForNonCollectionItems_IsWorking()
    {
        const string customerNameInit = "CLIPPERnet";
        const string customerNameValidUpdate = "CLIPPERnet GmbH";

        var offer = new Offer
        {
            Customer = new Customer
            {
                Name = customerNameInit
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            // EF Default
            dbContext.Add(offer);
            await dbContext.SaveChangesAsync();
        }

        var offerUpdate = new Offer
        {
            Id = offer.Id,
            Customer = new Customer
            {
                Id = offer.Customer.Id,
                Name = customerNameValidUpdate
            },
            Project = new Project
            {
                Name = "New Project",
                Customer = new Customer
                {
                    Id = offer.Customer.Id,
                    Name = "String change not be persisted because of Association"
                }
            }
        };

        await using (var dbContext = new TestDbContext())
        {
            var graphTracker = new DetachedGraphTracker(dbContext);
            await graphTracker.TrackGraphAsync(offerUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new TestDbContext())
        {
            var offerFromDb = await dbContext
                .Set<Offer>()
                .Include(a => a.Customer)
                .Include(a => a.Project)
                .ThenInclude(b => b!.Customer)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(offerFromDb.Customer!.Name, Is.EqualTo(customerNameValidUpdate));
                Assert.That(offerFromDb.Project!.Customer!.Name, Is.EqualTo(customerNameValidUpdate));
                Assert.That(offerFromDb.Project.Customer.Id, Is.EqualTo(offerFromDb.Customer.Id));
            });
        }
    }
}