using CompanyStructAPI.Contexts;
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
        public ActionResult GetDepartments()
        {
            var departments = _context.Departments.ToList();
            return Ok(departments);
        }
        [HttpGet("{id}")]
        public ActionResult GetDepartmentById(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
        [HttpPost]
        public ActionResult CreateDepartment([FromBody] Department department)
        {
            if (!EmployeeExists(department.ManagerID))
            {
                return BadRequest($"Employee with id: {department.ManagerID} not found. Cannot create department.");
            }
            _context.Departments.Add(department);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                Department newDepartment = new Department
                {
                    Name = updatedDepartment.Name,
                    Code = updatedDepartment.Code,
                    ProjectID = updatedDepartment.ProjectID,
                    ManagerID = updatedDepartment.ManagerID
                };
                return CreateDepartment(newDepartment);
            }
            if (!EmployeeExists(updatedDepartment.ManagerID))
            {
                return BadRequest($"Employee with id: {updatedDepartment.ManagerID} not found. Cannot update department.");
            }
            department.Name = updatedDepartment.Name;
            department.Code = updatedDepartment.Code;
            department.ProjectID = updatedDepartment.ProjectID;
            department.ManagerID = updatedDepartment.ManagerID;
            _context.SaveChanges();
            return Ok(department);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return Ok();
        }

        private bool EmployeeExists(int id)
        {
            Employee employee = _context.Employees.Find(id);
            return employee != null;
        }
    }
}
