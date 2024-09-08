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

        [HttpGet("search")]
        public ActionResult GetProjects([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? divisionId, [FromQuery] int? managerId)
        {
            var projects = _context.Projects.AsQueryable();
            if (id != null)
            {
                projects = projects.Where(p => p.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                projects = projects.Where(p => p.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                projects = projects.Where(p => p.Code.StartsWith(code));
            }
            if (divisionId != null)
            {
                projects = projects.Where(p => p.DivisionID == divisionId);
            }
            if (managerId != null)
            {
                projects = projects.Where(p => p.ManagerID == managerId);
            }
            return Ok(projects.ToList());
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

        [TypeFilter(typeof(ProjectUpdateValidationFilter))]
        [HttpPut("search")]
        public ActionResult SearchAndUpdateProject([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? divisionId, [FromQuery] int? managerId, [FromBody] Project updatedProject)
        {
            var projects = _context.Projects.AsQueryable();
            if (id != null)
            {
                projects = projects.Where(p => p.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                projects = projects.Where(p => p.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                projects = projects.Where(p => p.Code.StartsWith(code));
            }
            if (divisionId != null)
            {
                projects = projects.Where(p => p.DivisionID == divisionId);
            }
            if (managerId != null)
            {
                projects = projects.Where(p => p.ManagerID == managerId);
            }
            var projectList = projects.ToList();
            if (projectList.Count > 1)
            {
                ModelState.AddModelError("project", "Multiple projects found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new BadRequestObjectResult(problemDetails);
            }
            if (projectList.Count == 0)
            {
                ModelState.AddModelError("project", "Project not found.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                return new BadRequestObjectResult(problemDetails);
            }
            return UpdateProject(projectList.First().Id, updatedProject);
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

        [TypeFilter(typeof(ProjectSearchAndDeleteValidationFilter))]
        [HttpDelete("search")]
        public ActionResult SearchAndDeleteProject([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? code, [FromQuery] int? divisionId, [FromQuery] int? managerId)
        {
            var projects = _context.Projects.AsQueryable();
            if (id != null)
            {
                projects = projects.Where(p => p.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                projects = projects.Where(p => p.Name.StartsWith(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                projects = projects.Where(p => p.Code.StartsWith(code));
            }
            if (divisionId != null)
            {
                projects = projects.Where(p => p.DivisionID == divisionId);
            }
            if (managerId != null)
            {
                projects = projects.Where(p => p.ManagerID == managerId);
            }
            var projectList = projects.ToList();
            return DeleteProject(projectList.First().Id);
        }
    }
}
