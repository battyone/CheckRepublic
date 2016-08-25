using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Knapcode.CheckRepublic.Logic.Entities
{
    public class CheckContextFactory : IDbContextFactory<CheckContext>
    {
        public CheckContext Create(DbContextFactoryOptions options)
        {
            var databasePath = Path.Combine(options.ApplicationBasePath, "CheckRepublic.sqlite");

            var builder = new DbContextOptionsBuilder<CheckContext>();
            builder.UseSqlite($"Filename={databasePath}");

            return new CheckContext(builder.Options);
        }
    }
}
