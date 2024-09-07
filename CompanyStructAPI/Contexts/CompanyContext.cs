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
        public DbSet<Company> Companies { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }

        public bool EmployeeExists(int id)
        {
            return Employees.Any(e => e.Id == id);
        }

        public bool EmployeeParametersExists(string firstName, string lastName, string phone, string email)
        {
            return Employees.Any(e => e.FirstName == firstName && e.LastName == lastName && e.Phone == phone && e.Email == email);
        }

        public Employee? GetEmployeeByParameters(string firstName, string lastName, string phone, string email)
        {
            return Employees.FirstOrDefault(e => e.FirstName == firstName && e.LastName == lastName && e.Phone == phone && e.Email == email);
        }

        public bool CompanyExists(int id)
        {
            return Companies.Any(c => c.Id == id);
        }

        public bool CompanyParametersExists(string name, string code)
        {
            return Companies.Any(c => c.Name == name && c.Code == code);
        }

        public Company? GetCompanyByParameters(string name, string code)
        {
            return Companies.FirstOrDefault(c => c.Name == name && c.Code == code);
        }

        public bool DivisionExists(int id)
        {
            return Divisions.Any(d => d.id == id);
        }

        public bool DivisionParametersExists(string name, string code)
        {
            return Divisions.Any(d => d.Name == name && d.Code == code);
        }

        public Division? GetDivisionByParameters(string name, string code)
        {
            return Divisions.FirstOrDefault(d => d.Name == name && d.Code == code);
        }

        public bool ProjectExists(int id) {
            return Projects.Any(p => p.Id == id);
        }

        public bool ProjectParametersExists(string name, string code)
        {
            return Projects.Any(p => p.Name == name && p.Code == code);
        }

        public Project? GetProjectByParameters(string name, string code)
        {
            return Projects.FirstOrDefault(p => p.Name == name && p.Code == code);
        }
    }
}
