using CompanyStructAPI.Contexts;
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
            var projects = _context.Projects.ToList();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public ActionResult GetProjectById(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost]
        public ActionResult CreateProject([FromBody] Project project)
        {
            if (!_context.EmployeeExists(project.ManagerID))
            {
                return BadRequest($"Employee with id: {project.ManagerID} not found. Cannot create project.");
            }
            if (!_context.DivisionExists(project.DivisionID))
            {
                return BadRequest($"Division with id: {project.DivisionID} not found. Cannot create project.");
            }
            _context.Projects.Add(project);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProject(int id, [FromBody] Project updatedProject)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                Project newProject = new Project
                {
                    Name = updatedProject.Name,
                    Code = updatedProject.Code,
                    DivisionID = updatedProject.DivisionID,
                    ManagerID = updatedProject.ManagerID
                };
                return CreateProject(newProject);
            }
            if (!_context.EmployeeExists(updatedProject.ManagerID))
            {
                return BadRequest($"Employee with id: {updatedProject.ManagerID} not found. Cannot update project.");
            }
            if (!_context.DivisionExists(updatedProject.DivisionID))
            {
                return BadRequest($"Division with id: {updatedProject.DivisionID} not found. Cannot update project.");
            }
            project.Name = updatedProject.Name;
            project.Code = updatedProject.Code;
            project.DivisionID = updatedProject.DivisionID;
            project.ManagerID = updatedProject.ManagerID;
            _context.SaveChanges();
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProject(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            if (_context.Departments.Any(department => department.ProjectID == id))
            {
                return BadRequest("Cannot delete project with associated departments");
            }
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return Ok();
        }
    }
}
