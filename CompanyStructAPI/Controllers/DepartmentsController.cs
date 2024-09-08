using CompanyStructAPI.Contexts;
using CompanyStructAPI.Filters.DepartmentFilters;
using CompanyStructAPI.Filters.DivisionFilters;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public DepartmentsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            return Ok(_context.Departments.ToList());
        }

        [TypeFilter(typeof(DepartmentIdValidationFilter))]
        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            return Ok(_context.Departments.Find(id));
        }

        [HttpGet("search")]
        public IActionResult GetDepartments([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? projectID, [FromQuery] int? managerID)
        {
            var departments = _context.Departments.AsQueryable();
            if (id != null)
            {
                departments = departments.Where(d => d.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                departments = departments.Where(d => d.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                departments = departments.Where(d => d.Code.StartsWith(code));
            }
            if (projectID != null)
            {
                departments = departments.Where(d => d.ProjectID == projectID);
            }
            if (managerID != null)
            {
                departments = departments.Where(d => d.ManagerID == managerID);
            }
            return Ok(departments.ToList());
        }

        [TypeFilter(typeof(DepartmentCreateValidationFilter))]
        [HttpPost]
        public IActionResult CreateDepartment([FromBody] Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }

        [TypeFilter(typeof(DepartmentIdValidationFilter))]
        [TypeFilter(typeof(DepartmentUpdateValidationFilter))]
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            var department = _context.Departments.Find(id);
            department.Name = updatedDepartment.Name;
            department.Code = updatedDepartment.Code;
            department.ProjectID = updatedDepartment.ProjectID;
            department.ManagerID = updatedDepartment.ManagerID;
            _context.SaveChanges();
            return Ok(department);
        }

        [TypeFilter(typeof(DepartmentUpdateValidationFilter))]
        [HttpPut("search")]
        public IActionResult UpdateDepartments([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? projectID, [FromQuery] int? managerID, [FromBody] Department updatedDepartment)
        {
            var departments = _context.Departments.AsQueryable();
            if (id != null)
            {
                departments = departments.Where(d => d.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                departments = departments.Where(d => d.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                departments = departments.Where(d => d.Code.StartsWith(code));
            }
            if (projectID != null)
            {
                departments = departments.Where(d => d.ProjectID == projectID);
            }
            if (managerID != null)
            {
                departments = departments.Where(d => d.ManagerID == managerID);
            }
            var departmentList = departments.ToList();
            if (departmentList.Count > 1)
            {
                ModelState.AddModelError("department", "Multiple departments found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            }
            if (departmentList.Count == 0)
            {
                ModelState.AddModelError("department", "Department not found.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return new BadRequestObjectResult(problemDetails);
            }
            return UpdateDepartment(departmentList.First().Id, updatedDepartment);
        }

        [TypeFilter(typeof(DepartmentIdValidationFilter))]
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.Find(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("search")]
        public IActionResult SearchAndDelete([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? projectID, [FromQuery] int? managerID)
        {
            var departments = _context.Departments.AsQueryable();
            if (id != null)
            {
                departments = departments.Where(d => d.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                departments = departments.Where(d => d.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                departments = departments.Where(d => d.Code.StartsWith(code));
            }
            if (projectID != null)
            {
                departments = departments.Where(d => d.ProjectID == projectID);
            }
            if (managerID != null)
            {
                departments = departments.Where(d => d.ManagerID == managerID);
            }
            var departmentList = departments.ToList();
            if (departmentList.Count > 1)
            {
                ModelState.AddModelError("department", "Multiple departments found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            }
            if (departmentList.Count == 0)
            {
                ModelState.AddModelError("department", "Department not found.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return new BadRequestObjectResult(problemDetails);
            }
            return DeleteDepartment(departmentList.First().Id);
        }
    }
}
