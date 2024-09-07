using CompanyStructAPI.Contexts;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public EmployeesController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Employee>> GetEmployees()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPost]
        public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }
        [HttpPut("{id}")]
        public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                Employee newEmployee = new Employee
                {
                    FirstName = updatedEmployee.FirstName,
                    LastName = updatedEmployee.LastName,
                    Phone = updatedEmployee.Phone,
                    Email = updatedEmployee.Email
                };
                return CreateEmployee(newEmployee);
            }
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Phone = updatedEmployee.Phone;
            employee.Email = updatedEmployee.Email;
            _context.SaveChanges();
            return Ok(employee);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            bool isReferenceInCompany = _context.Companies.Any(c => c.CeoID == id);
            bool isReferenceInDivision = _context.Divisions.Any(d => d.DirectorID == id);
            bool isReferenceInProject = _context.Projects.Any(p => p.ManagerID == id);
            bool isReferenceInDepartment = _context.Departments.Any(d => d.ManagerID == id);
            string errorMessage = "";
            if (isReferenceInCompany)
            {
                errorMessage += "Cannot delete, employee is a CEO of a company.\n";
            }
            if (isReferenceInDivision)
            {
                errorMessage += "Cannot delete, employee is a director of a division.\n";
            }
            if (isReferenceInProject)
            {
                errorMessage += "Cannot delete, employee is a manager of a project.\n";
            }
            if (isReferenceInDepartment)
            {
                errorMessage += "Cannot delete, employee is a manager of a department.\n";
            }
            if (errorMessage != "")
            {
                return BadRequest(errorMessage.Trim());
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
