using CompanyStructAPI.Contexts;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.ProjectFilters
{
    public class ProjectDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public ProjectDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var id = context.ActionArguments["id"] as int?;
            if (_context.Departments.Any(department => department.ProjectID == id))
            {
                context.ModelState.AddModelError("project", "Cannot delete, project is referenced in Department");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }

    public class ProjectSearchAndDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public ProjectSearchAndDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var projects = _context.Projects.AsQueryable();
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as int?;
                if (id != null)
                {
                    projects = projects.Where(p => p.Id == id);
                }
            }
            if (context.ActionArguments.ContainsKey("name"))
            {
                var name = context.ActionArguments["name"] as string;
                if (!string.IsNullOrEmpty(name))
                {
                    projects = projects.Where(p => p.Name.StartsWith(name));
                }
            }
            if (context.ActionArguments.ContainsKey("code"))
            {
                var code = context.ActionArguments["code"] as string;
                if (!string.IsNullOrEmpty(code))
                {
                    projects = projects.Where(p => p.Code.StartsWith(code));
                }
            }
            if (context.ActionArguments.ContainsKey("divisionId"))
            {
                var divisionId = context.ActionArguments["divisionId"] as int?;
                if (divisionId != null)
                {
                    projects = projects.Where(p => p.DivisionID == divisionId);
                }
            }
            if (context.ActionArguments.ContainsKey("managerId"))
            {
                var managerId = context.ActionArguments["managerId"] as int?;
                if (managerId != null)
                {
                    projects = projects.Where(p => p.ManagerID == managerId);
                }
            }            
            var projectList = projects.ToList();
            if (projectList.Count > 1)
            {
                context.ModelState.AddModelError("project", "Multiple projects found. Please provide more specific search criteria.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            if (projectList.Count == 0)
            {
                context.ModelState.AddModelError("project", "Project not found.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            if (_context.Departments.Any(department => department.ProjectID == projectList.First().Id))
            {
                context.ModelState.AddModelError("project", "Cannot delete, project is referenced in Department");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
