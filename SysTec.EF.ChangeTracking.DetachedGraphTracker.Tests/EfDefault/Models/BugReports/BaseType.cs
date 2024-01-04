using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.BugReports;

public abstract class BaseType
{
        [Key]
        public int Id { get; set; }
        
        [Timestamp]
        [ConcurrencyCheck]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("xmin", TypeName = "xid")]
        public uint ConcurrencyToken { get; set; }

        public string Discriminator { get; set; }
}