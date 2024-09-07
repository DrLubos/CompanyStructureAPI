using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CompanyStructAPI.Models;

namespace CompanyStructAPI.Filters.ProjectFilters
{
    public class ProjectCreateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public ProjectCreateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("project"))
            {
                var project = context.ActionArguments["project"] as Project;
                if (project != null)
                {
                    if (!_context.EmployeeExists(project.ManagerID))
                    {
                        context.ModelState.AddModelError("project", $"Employee with id: {project.ManagerID} not found. Cannot create division.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (!_context.DivisionExists(project.DivisionID))
                    {
                        context.ModelState.AddModelError("project", $"Division with id: {project.DivisionID} not found. Cannot create project.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (project.Name == null || project.Code == null)
                    {
                        context.ModelState.AddModelError("project", "Project is missing required fields.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                    if (project.Name.Length > 100 || project.Code.Length > 100)
                    {
                        context.ModelState.AddModelError("project", "Project fields are too long.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    if (_context.ProjectParametersExists(project.Name, project.Code))
                    {
                        context.ModelState.AddModelError("project", "Project with this name and code already exists.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                }
            }
        }
    }
}
