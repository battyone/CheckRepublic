using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckContextFactory : IDesignTimeDbContextFactory<CheckContext>
    {
        public CheckContext CreateDbContext(string[] args)
        {
            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "CheckRepublic.sqlite3");

            var builder = new DbContextOptionsBuilder<CheckContext>();
            builder.UseSqlite($"Filename={databasePath}");

            return new CheckContext(builder.Options);
        }
    }
}
