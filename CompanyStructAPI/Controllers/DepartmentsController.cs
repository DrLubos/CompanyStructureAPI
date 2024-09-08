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
        public ActionResult GetDepartments()
        {
            return Ok(_context.Departments.ToList());
        }

        [TypeFilter(typeof(DepartmentIdValidationFilter))]
        [HttpGet("{id}")]
        public ActionResult GetDepartmentById(int id)
        {
            return Ok(_context.Departments.Find(id));
        }

        [TypeFilter(typeof(DepartmentCreateValidationFilter))]
        [HttpPost]
        public ActionResult CreateDepartment([FromBody] Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }

        [TypeFilter(typeof(DepartmentIdValidationFilter))]
        [TypeFilter(typeof(DepartmentUpdateValidationFilter))]
        [HttpPut("{id}")]
        public ActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            var department = _context.Departments.Find(id);
            department.Name = updatedDepartment.Name;
            department.Code = updatedDepartment.Code;
            department.ProjectID = updatedDepartment.ProjectID;
            department.ManagerID = updatedDepartment.ManagerID;
            _context.SaveChanges();
            return Ok(department);
        }

        [TypeFilter(typeof(DepartmentIdValidationFilter))]
        [HttpDelete("{id}")]
        public ActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.Find(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return Ok();
        }
    }
}
