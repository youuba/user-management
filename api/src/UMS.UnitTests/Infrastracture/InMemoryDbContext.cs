using Microsoft.EntityFrameworkCore;
using UMS.Infrastructure.Data;

namespace UMS.UnitTests.Infrastracture
{
    internal class InMemoryDbContext : AppDbContext
    {
        public InMemoryDbContext()
        {
        }
        public InMemoryDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
    }
}
