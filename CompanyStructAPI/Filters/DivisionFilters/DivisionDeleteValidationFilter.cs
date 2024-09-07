using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CompanyStructAPI.Filters.DivisionFilters
{
    public class DivisionDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DivisionDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var id = context.ActionArguments["id"] as int?;
            if (_context.Projects.Any(project => project.DivisionID == id))
            {
                context.ModelState.AddModelError("company", "Cannot delete, division is referenced in Project");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
