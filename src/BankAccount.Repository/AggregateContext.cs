using System.Diagnostics.CodeAnalysis;
using Bank.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Bank.Repository
{
    [ExcludeFromCodeCoverage]
    public class AggregateContext : DbContext
    {
        public AggregateContext(DbContextOptions<AggregateContext> options)
            : base(options)
        {
        }
        
        [ExcludeFromCodeCoverage]
        public DbSet<EventDescriptor> Events { get; set; }
    }
}