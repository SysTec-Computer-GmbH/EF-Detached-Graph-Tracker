using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests;

public class DbContextBase : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
                @"User ID=postgres;Password=;Server=localhost;Port=5432;Database=SysTec_DetachedGraphTracker;Pooling=true;Include Error Detail=true;Command Timeout=0")
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }
}