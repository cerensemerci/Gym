using Microsoft.EntityFrameworkCore;

namespace Basics.Models
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            
        }
    }
}