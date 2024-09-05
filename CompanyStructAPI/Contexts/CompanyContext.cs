using CompanyStructAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyStructAPI.Contexts
{
    public class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> optoins) : base(optoins)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
