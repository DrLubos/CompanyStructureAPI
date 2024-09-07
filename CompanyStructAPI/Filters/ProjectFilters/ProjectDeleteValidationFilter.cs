using CompanyStructAPI.Contexts;
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
}
