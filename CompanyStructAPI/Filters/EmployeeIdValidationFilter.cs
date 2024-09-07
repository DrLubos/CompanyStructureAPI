using CompanyStructAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyStructAPI.Filters
{
    public class EmployeeIdValidationFilter : ActionFilterAttribute
    {
        private readonly CompanyContext _context;

        public EmployeeIdValidationFilter(CompanyContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var employeeId = context.ActionArguments["id"] as int?;
                if (employeeId != null && employeeId.HasValue)
                {
                    if (employeeId.Value < 0)
                    {
                        context.ModelState.AddModelError("id", "Employee ID is invalid.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                    else if (!_context.EmployeeExists(employeeId.Value))
                    {
                        context.ModelState.AddModelError("id", "Employee ID does not exist.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                        return;
                    }
                }
            }
        }
    }
}
