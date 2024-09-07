using CompanyStructAPI.Contexts;
using CompanyStructAPI.Filters.DivisionFilters;
using CompanyStructAPI.Filters.ProjectFilters;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyStructAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ProjectsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetProjects()
        {
            return Ok(_context.Projects.ToList());
        }

        [TypeFilter(typeof(ProjectIdValidationFilter))]
        [HttpGet("{id}")]
        public ActionResult GetProjectById(int id)
        {
            return Ok(_context.Projects.Find(id));
        }

        [TypeFilter(typeof(ProjectCreateValidationFilter))]
        [HttpPost]
        public ActionResult CreateProject([FromBody] Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [TypeFilter(typeof(ProjectIdValidationFilter))]
        [TypeFilter(typeof(ProjectUpdateValidationFilter))]
        [HttpPut("{id}")]
        public ActionResult UpdateProject(int id, [FromBody] Project updatedProject)
        {
            var project = _context.Projects.Find(id);
            project.Name = updatedProject.Name;
            project.Code = updatedProject.Code;
            project.DivisionID = updatedProject.DivisionID;
            project.ManagerID = updatedProject.ManagerID;
            _context.SaveChanges();
            return Ok(project);
        }

        [TypeFilter(typeof(ProjectIdValidationFilter))]
        [TypeFilter(typeof(ProjectDeleteValidationFilter))]
        [HttpDelete("{id}")]
        public ActionResult DeleteProject(int id)
        {
            var project = _context.Projects.Find(id);
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return Ok();
        }
    }
}
