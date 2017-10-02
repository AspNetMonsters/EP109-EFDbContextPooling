using Microsoft.EntityFrameworkCore;

namespace EfContextPooling.Data
{
    public class EmployeeContext : DbContext
    {
        
        public EmployeeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
