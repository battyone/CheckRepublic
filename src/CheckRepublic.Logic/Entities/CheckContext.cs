using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckContext : DbContext
    {
        public CheckContext(DbContextOptions<CheckContext> options) : base(options)
        {
        }

        public DbSet<CheckEntity> CheckEntities { get; set; }
        public DbSet<CheckResultEntity> CheckResultEntities { get; set; }
    }
}
