using CompanyStructAPI.Contexts;
using CompanyStructAPI.Filters.EmployeeFilters;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpGet("search")]
        public IActionResult GetEmployees([FromQuery] int? id, [FromQuery] string? title, [FromQuery] string? firstname, [FromQuery] string? lastname, [FromQuery] string? phone, [FromQuery] string? email)
        {
            var employees = _context.Employees.AsQueryable();
            if (id != null)
            {
                employees = employees.Where(e => e.Id == id);
            }
            if (!string.IsNullOrEmpty(title))
            {
                employees = employees.Where(e => e.Title == title);
            }
            if (!string.IsNullOrEmpty(firstname))
            {
                employees = employees.Where(e => e.FirstName.StartsWith(firstname));
            }
            if (!string.IsNullOrEmpty(lastname))
            {
                employees = employees.Where(e => e.LastName.StartsWith(lastname));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                employees = employees.Where(e => e.Phone.StartsWith(phone));
            }
            if (!string.IsNullOrEmpty(email))
            {
                employees = employees.Where(e => e.Email.StartsWith(email));
            }
            return Ok(employees.ToList());
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

        [HttpPut("search")]
        public IActionResult SearchAndUpdate([FromQuery] int? id, [FromQuery] string? title, [FromQuery] string? firstname, [FromQuery] string? lastname, [FromQuery] string? phone, [FromQuery] string? email, [FromBody] Employee updatedEmployee)
        {
            var employees = _context.Employees.AsQueryable();
            if (id != null)
            {
                employees = employees.Where(e => e.Id == id);
            }
            if (!string.IsNullOrEmpty(title))
            {
                employees = employees.Where(e => e.Title == title);
            }
            if (!string.IsNullOrEmpty(firstname))
            {
                employees = employees.Where(e => e.FirstName.StartsWith(firstname));
            }
            if (!string.IsNullOrEmpty(lastname))
            {
                employees = employees.Where(e => e.LastName.StartsWith(lastname));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                employees = employees.Where(e => e.Phone.StartsWith(phone));
            }
            if (!string.IsNullOrEmpty(email))
            {
                employees = employees.Where(e => e.Email.StartsWith(email));
            }
            var employeeList = employees.ToList();
            if (employeeList.Count > 1)
            {
                ModelState.AddModelError("employee", "Multiple employees found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            }
            if (employeeList.Count == 0)
            {
                ModelState.AddModelError("employee", "Employee not found.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return new NotFoundObjectResult(problemDetails);
            }
            return UpdateEmployee(employeeList.First().Id, updatedEmployee);
        }

        [TypeFilter(typeof(EmployeeIdValidationFilter))]
        [TypeFilter(typeof(EmployeeDeleteValidationFilter))]
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
