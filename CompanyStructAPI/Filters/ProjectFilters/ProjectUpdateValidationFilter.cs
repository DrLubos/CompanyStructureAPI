using CompanyStructAPI.Contexts;
using CompanyStructAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.ProjectFilters
{
    public class ProjectUpdateValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public ProjectUpdateValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var updatedProject = context.ActionArguments["updatedProject"] as Project;
            if (updatedProject == null)
            {
                context.ModelState.AddModelError("project", "Project is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                if (!_context.EmployeeExists(updatedProject.ManagerID))
                {
                    context.ModelState.AddModelError("project", $"Employee with id: {updatedProject.ManagerID} not found. Cannot update project.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (!_context.DivisionExists(updatedProject.DivisionID))
                {
                    context.ModelState.AddModelError("project", $"Division with id: {updatedProject.DivisionID} not found. Cannot update project.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                if (updatedProject.Name == null || updatedProject.Code == null)
                {
                    context.ModelState.AddModelError("project", "Project is missing required fields.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
                if (updatedProject.Name.Length > 100 || updatedProject.Code.Length > 100)
                {
                    context.ModelState.AddModelError("project", "Project fields are too long.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                var project = _context.GetProjectByParameters(updatedProject.Name, updatedProject.Code);
                if (project != null && project.DivisionID == updatedProject.DivisionID && project.ManagerID == updatedProject.ManagerID)
                {
                    context.ModelState.AddModelError("project", "Project with entered name, code, divisionID and managerID already exists.");
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
