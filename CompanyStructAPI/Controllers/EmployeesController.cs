using CompanyStructAPI.Contexts;
using CompanyStructAPI.Filters;
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
        public IActionResult GetEmployees()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }

        [TypeFilter(typeof(EmployeeIdValidationFilter))]
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            return Ok(_context.Employees.Find(id));
        }

        [TypeFilter(typeof(EmployeeCreateValidationFilter))]
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        [TypeFilter(typeof(EmployeeIdValidationFilter))]
        [TypeFilter(typeof(EmployeeUpdateValidationFilter))]
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            var employee = _context.Employees.Find(id);    
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Phone = updatedEmployee.Phone;
            employee.Email = updatedEmployee.Email;
            _context.SaveChanges();
            return Ok(employee);
        }

        [TypeFilter(typeof(EmployeeIdValidationFilter))]
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            bool isReferenceInCompany = _context.Companies.Any(c => c.CeoID == id);
            bool isReferenceInDivision = _context.Divisions.Any(d => d.DirectorID == id);
            bool isReferenceInProject = _context.Projects.Any(p => p.ManagerID == id);
            bool isReferenceInDepartment = _context.Departments.Any(d => d.ManagerID == id);
            string errorMessage = "";
            if (isReferenceInCompany)
            {
                errorMessage += "Cannot delete, employee is a CEO of a company.";
            }
            if (isReferenceInDivision)
            {
                errorMessage += "Cannot delete, employee is a director of a division.";
            }
            if (isReferenceInProject)
            {
                errorMessage += "Cannot delete, employee is a manager of a project.";
            }
            if (isReferenceInDepartment)
            {
                errorMessage += "Cannot delete, employee is a manager of a department.";
            }
            if (errorMessage != "")
            {
                ModelState.AddModelError("employee", errorMessage);
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                var result = new BadRequestObjectResult(problemDetails);
                return result;
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
