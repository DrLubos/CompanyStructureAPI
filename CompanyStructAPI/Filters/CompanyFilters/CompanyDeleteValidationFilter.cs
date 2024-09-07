using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.CompanyFilters
{
    public class CompanyDeleteValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public CompanyDeleteValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var id = context.ActionArguments["id"] as int?;
            if (_context.Divisions.Any(division => division.CompanyID == id))
            {
                context.ModelState.AddModelError("company", "Cannot delete, company is referenced in Division");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
