using Microsoft.EntityFrameworkCore;

namespace Bank.Repository
{
    public class AggregateContext : DbContext
    {
        public AggregateContext(DbContextOptions<AggregateContext> options)
            : base(options)
        {
        }
        
        public DbSet<EventDescriptor> Events { get; set; }
    }
}