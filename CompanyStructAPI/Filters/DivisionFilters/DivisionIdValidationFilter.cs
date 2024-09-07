using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters.DivisionFilters
{
    public class DivisionIdValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public DivisionIdValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var divisionId = context.ActionArguments["id"] as int?;
                if (divisionId != null && divisionId.HasValue)
                {
                    if (divisionId.Value < 0)
                    {
                        context.ModelState.AddModelError("id", "Division ID is invalid.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    else if (!_context.DivisionExists(divisionId.Value))
                    {
                        context.ModelState.AddModelError("id", "Division ID does not exist.");
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
