using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructAPI.Filters.ProjectFilters
{
    public class ProjectIdValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public ProjectIdValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var projectId = context.ActionArguments["id"] as int?;
                if (projectId != null && projectId.HasValue)
                {
                    if (projectId.Value < 0)
                    {
                        context.ModelState.AddModelError("id", "Project ID is invalid.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    else if (!_context.ProjectExists(projectId.Value))
                    {
                        context.ModelState.AddModelError("id", "Project ID does not exist.");
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
