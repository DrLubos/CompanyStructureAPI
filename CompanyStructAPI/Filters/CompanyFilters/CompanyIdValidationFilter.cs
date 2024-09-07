using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.CompanyFilters
{
    public class CompanyIdValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public CompanyIdValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var companyId = context.ActionArguments["id"] as int?;
                if (companyId != null && companyId.HasValue)
                {
                    if (companyId.Value < 0)
                    {
                        context.ModelState.AddModelError("id", "Company ID is invalid.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    else if (!_context.CompanyExists(companyId.Value))
                    {
                        context.ModelState.AddModelError("id", "Company ID does not exist.");
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
